﻿using Microsoft.EntityFrameworkCore;
using WebCustomerFeedbackSystem.Models;
using System;

namespace WebCustomerFeedbackSystem.EF
{
    public class FeedbackEF : IFeedback
    {
        private readonly CustomerFeedbackSystemContext _dbContext;
        public FeedbackEF(CustomerFeedbackSystemContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Feedback Add(Feedback entity)
        {
            try
            {
                _dbContext.Feedbacks.Add(entity);
                _dbContext.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Feedback> GetAll()
        {
            return _dbContext.Feedbacks
               .FromSqlRaw("SELECT * FROM Feedback")
               .ToList();
        }

        public Feedback GetById(int id)
        {
            var result = _dbContext.Feedbacks.Find(id);
            if (result == null)
            {
                throw new Exception("Feedback not found");
            }
            return result;
        }

        public Feedback Update(Feedback entity)
        {
            try
            {
                var updateFeedback = GetById(entity.FeedbackId);
                updateFeedback.Status = entity.Status;
                _dbContext.SaveChanges();
                return updateFeedback;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
