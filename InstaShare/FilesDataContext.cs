using InstaShare.Application.Entities;
using InstaShare.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InstaShare.Infrastructure;

public class FilesDataContext : DbContext, IFilesDataContext
{
    public FilesDataContext()
    {
    }

    public FilesDataContext(DbContextOptions options) : base(options)
    {
    }


    public DbSet<ContactEntity> Contacts { get; set; } = null!;

    public DbSet<OfficeEntity> Offices { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OfficeEntity>();

        modelBuilder.Entity<OfficeEntity>()
            .HasData(
                new OfficeEntity
                {
                    Id = new Guid("ff0c022e-1aff-4ad8-2231-08db0378ac98"),
                    Name = "Default Office"
                }
            );

        modelBuilder.Entity<ContactEntity>();

        modelBuilder.Entity<ContactEntity>()
            .HasData(
                new ContactEntity
                {
                    Id = new Guid("c00b9ff3-b1b6-42fe-8b5a-4c28408fb64a"),
                    FirstName = "Alejandro",
                    LastName = "Alfonso",
                    Email = "aalfonso@gmail.com"
                }
                , new ContactEntity
                {
                    Id = new Guid("1ec2d3f7-8aa8-4bf5-91b8-045378919049"),
                    FirstName = "Mark",
                    LastName = "Costello",
                    Email = "mcostello@gmail.com"
                }
            );

        modelBuilder.Entity<ContactOfficeRelation>()
            .HasKey(x => new { x.ContactId, x.OfficeId });

        modelBuilder.Entity<ContactEntity>()
            .HasMany(x => x.ContactOffices)
            .WithOne(x => x.Contact)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OfficeEntity>()
            .HasMany(x => x.ContactOffices)
            .WithOne(x => x.Office)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ContactOfficeRelation>()
            .HasData(
                new ContactOfficeRelation
                {
                    ContactId = new Guid("c00b9ff3-b1b6-42fe-8b5a-4c28408fb64a"),
                    OfficeId = new Guid("9ca78c33-4590-43c1-a7c4-55696a5efd44"),
                });
    }

    // Basic Code to add more sample data
    // It creates 5 Random Offices and 20 random Contacts
    // 1 Office representing to Continuum and
    // 1 Office representing Granite organizations are created
    // 10 of the contacts are related to Continuum and the other to both Granite and Continuum
    public void SeedInitialData()
    {
        var offices = new List<OfficeEntity>();
        var contacts = new List<ContactEntity>();

        for (int i = 1; i <= 5; ++i)
        {
            var office = new OfficeEntity
            {
                Id = Guid.NewGuid(),
                Name = $"Office {i}"
            };

            offices.Add(office);
        }

        var granite = new OfficeEntity
        {
            Id = Guid.NewGuid(),
            Name = "Granite Group"
        };

        var continuum = new OfficeEntity
        {
            Id = Guid.NewGuid(),
            Name = "Continuum"
        };

        offices.Add(granite);
        offices.Add(continuum);

        for (int i = 0; i < 10; ++i)
        {
            var contact = new ContactEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "Continuum-Test",
                LastName = $"Contact-{i}"
            };

            contact.Email = $"{contact.FirstName.ToLower()}.{contact.LastName.ToLower()}@wearecontinuum.com";

            contact.ContactOffices = [
                new() { OfficeId = granite.Id, ContactId = contact.Id },
                new() { OfficeId = continuum.Id, ContactId = contact.Id },
            ];

            contacts.Add(contact);
        }

        for (int i = 0; i < 10; ++i)
        {
            var contact = new ContactEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "Granite-Test",
                LastName = $"Contact-{i}",
            };

            contact.Email = $"{contact.FirstName.ToLower()}.{contact.LastName.ToLower()}@granite.ie";
            contact.ContactOffices = [
                new() { OfficeId = granite.Id, ContactId = contact.Id },
            ];

            contacts.Add(contact);
        }

        this.Offices.AddRange(offices);
        this.Contacts.AddRange(contacts);
    }
}