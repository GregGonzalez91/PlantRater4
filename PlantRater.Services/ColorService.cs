using PlantRater.Data;
using PlantRater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantRater.Services
{
    public class ColorService
    {
        private readonly Guid _userId;

        public ColorService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateColor(ColorCreate model)
        {
            var entity =
                new Color()
                {
                    OwnerId = _userId,
                    Name = model.Name,
                    CreatedUtc = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Colors.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ColorListItem> GetColors()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Colors
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new ColorListItem
                        {
                            ColorId = e.ColorId,
                            Name = e.Name,
                            CreatedUtc = e.CreatedUtc
                        }
                        );
                return query.ToArray();
            }
        }

        public ColorDetail GetColorById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Colors
                    .Single(e => e.ColorId == id && e.OwnerId == _userId);
                return
                    new ColorDetail
                    {
                        ColorId = entity.ColorId,
                        Name = entity.Name,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool UpdateColor(ColorEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Colors
                    .Single(e => e.ColorId == model.ColorId && e.OwnerId == _userId);

                entity.Name = model.Name;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteColor(int colorId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Colors
                    .Single(e => e.ColorId == colorId && e.OwnerId == _userId);

                ctx.Colors.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
