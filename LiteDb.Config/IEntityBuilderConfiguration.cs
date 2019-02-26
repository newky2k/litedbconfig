using System;
using LiteDB;

namespace LiteDb.Config
{
    /// <summary>
    /// Interface for configuring Entity for the BSONMapper entity builder
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface IEntityBuilderConfiguration<TEntity>
    {
        void Configure(EntityBuilder<TEntity> builder);
    }
}
