using PlantRater.Data;
using PlantRater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantRater.Services
{
    public class PlantService
    {
        private readonly Guid _userId;

        public PlantService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreatePlant(PlantCreate model)
        {
            var entity =
                new Plant()
                {
                    OwnerId = _userId,
                    Name = model.Name,
                    ColorId = model.ColorId,
                    Rating = model.Rating,
                    CreatedUtc = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Plants.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<PlantListItem> GetPlants()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Plants
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new PlantListItem
                        {
                            PlantId = e.PlantId,
                            Name = e.Name,
                            ColorId = e.ColorId,
                            Color = e.Color,
                            Rating = e.Rating,
                            CreatedUtc = e.CreatedUtc
                        }
                        );
                return query.ToArray();
            }
        }

        public List<Color> GetColor()
        {
            List<Color> colors = new List<Color>();
            using(var ctx = new ApplicationDbContext())
            {
                var clrs = ctx.Colors;
                foreach (Color c in clrs)
                {
                    colors.Add(new Color { ColorId = c.ColorId, Name = c.Name });
                }

                return colors;
            }
        }

        public PlantDetail GetPlantById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Plants
                    .Single(e => e.PlantId == id && e.OwnerId == _userId);
                return
                    new PlantDetail
                    {
                        PlantId = entity.PlantId,
                        Name = entity.Name,
                        ColorId = entity.ColorId,
                        Color = entity.Color,
                        Rating = entity.Rating,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool UpdatePlant(PlantEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Plants
                    .Single(e => e.PlantId == model.PlantId && e.OwnerId == _userId);

                entity.Name = model.Name;
                entity.Color = model.Color;
                entity.ColorId = model.ColorId;
                entity.Rating = model.Rating;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeletePlant(int plantId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Plants
                    .Single(e => e.PlantId == plantId && e.OwnerId == _userId);

                ctx.Plants.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
