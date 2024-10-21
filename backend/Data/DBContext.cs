using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class DBContext : IdentityDbContext<User>
    {
        public DBContext(DbContextOptions<DBContext> options)
        : base(options)
        {

        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<EventTemplate> EventTemplates { get; set; }        
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<Wallet> Wallets { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Detect if a new user is being added
            var newUser = ChangeTracker.Entries<User>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .FirstOrDefault();

            if (newUser != null)
            {
                // Create a wallet for the new user
                var wallet = new Wallet
                {
                    User = newUser,
                    Balance = 0
                };

                Wallets.Add(wallet);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
                

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.UserType)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Sessions)
                .WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascade

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Attendees)
                .WithOne(a => a.Event)
                .HasForeignKey(a => a.EventId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascade

            modelBuilder.Entity<Attendee>()
                .HasMany(a => a.Tickets)
                .WithOne(t => t.Attendee)
                .HasForeignKey(t => t.AttendeeId)
                .OnDelete(DeleteBehavior.Cascade); // Keep this if desired

            modelBuilder.Entity<Ticket>()
                .HasMany(t => t.Payments)
                .WithOne(p => p.Ticket)
                .HasForeignKey(p => p.TicketId)
                .OnDelete(DeleteBehavior.Cascade); // Keep this if desired

            modelBuilder.Entity<EventTemplate>()
                .HasMany(et => et.Events)
                .WithOne(e => e.Template)
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.SetNull); // Keep this if desired

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Event)
                .WithMany(e => e.Invitations)
                .HasForeignKey(i => i.EventId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascade

            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Reminders)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascade

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Event)
                .WithMany(e => e.Feedbacks)
                .HasForeignKey(f => f.EventId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascade

            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole{
                    Name = "Organizer",
                    NormalizedName = "ORGANIZER"
                },
                new IdentityRole{
                    Name = "Attendee",
                    NormalizedName = "ATTENDEE"
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);


        }



    }
}
