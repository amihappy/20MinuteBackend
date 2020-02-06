using _20MinuteBackend.Domain.Backend;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.Infrastructure
{

    internal class BackendEntityTypeConfiguration : IEntityTypeConfiguration<Backend>
    {
        public void Configure(EntityTypeBuilder<Backend> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StartTime).IsRequired();
            builder.Property(s => s.OrginalJson).IsRequired();

            builder.Property(s => s.OrginalJson).HasConversion(
                s => s.ToString(),
                s => JObject.Parse(s)
                );
        }
    }
}
