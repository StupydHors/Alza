﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.ImgUri)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(4000);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.LastModifiedAt)
            .IsRequired(false);
    }
}
