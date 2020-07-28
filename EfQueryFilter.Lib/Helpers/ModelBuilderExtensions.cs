﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EfQueryFilter.Lib.Helpers
{
    public static class ModelBuilderExtensions
    {
        private static readonly MethodInfo SetQueryFilterMethod;

        static ModelBuilderExtensions()
        {
            SetQueryFilterMethod = typeof(ModelBuilderExtensions)
              .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
              .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));
        }

        public static void SetQueryFilterOnAllEntities<TEntityInterface>(this ModelBuilder builder,
            Expression<Func<TEntityInterface, bool>> filterExpression)
        {
            foreach (var type in builder.Model.GetEntityTypes()
                .Where(t => t.BaseType == null)
                .Select(t => t.ClrType)
                .Where(t => typeof(TEntityInterface).IsAssignableFrom(t)))
            {
                builder.SetEntityQueryFilter<TEntityInterface>(type, filterExpression);
            }
        }

        private static void SetEntityQueryFilter<TEntityInterface>(this ModelBuilder builder,
            Type entityType, Expression<Func<TEntityInterface, bool>> filterExpression)
        {
            SetQueryFilterMethod
                .MakeGenericMethod(entityType, typeof(TEntityInterface))
                .Invoke(null, new object[] { builder, filterExpression });
        }

        private static void SetQueryFilter<TEntity, TEntityInterface>(this ModelBuilder builder,
            Expression<Func<TEntityInterface, bool>> filterExpression)
            where TEntityInterface : class
            where TEntity : class, TEntityInterface
        {
            var concreteExpression = filterExpression.Convert<TEntityInterface, TEntity>();
            builder.Entity<TEntity>().HasQueryFilter(concreteExpression);
        }
    }
}
