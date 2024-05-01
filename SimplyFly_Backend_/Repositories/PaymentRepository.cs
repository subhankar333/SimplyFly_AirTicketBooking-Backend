using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PaymentRepository : IRepository<int, Payment>
    {

        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(SimplyFlyDbContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Payment> Add(Payment item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation("Payment added with PaymentId " + item.PaymentId);
            return item;
        }

        public async Task<Payment> Delete(int paymentId)
        {
            var payment = await GetAsync(paymentId);
            if(payment != null)
            {
                _context.Remove(payment);
                _context.SaveChanges();
                _logger.LogInformation("Payment deleted with PaymentId" + paymentId);
                return payment;
            }

            throw new NoSuchPaymentException();
        }

        public async Task<Payment> GetAsync(int paymentId)
        {
            var payments = await GetAsync();
            var payment = payments.FirstOrDefault(p => p.PaymentId ==  paymentId);
            
            if(payments != null)
            {
                return payment;
            }

            throw new NoSuchPaymentException();
        }

        public async Task<List<Payment>> GetAsync()
        {
            var payments = _context.Payments.ToList();
            return payments;
        }

        public async Task<Payment> Update(Payment item)
        {
            var payment = await GetAsync(item.PaymentId);
            if(payment != null)
            {
                _context.Entry<Payment>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Payment updated with PaymentId {item.PaymentId}");
                return payment;
            }

            throw new NoSuchPaymentException();
        }
        
    }
}
