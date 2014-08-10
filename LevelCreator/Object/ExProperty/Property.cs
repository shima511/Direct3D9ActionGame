using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelCreator.Object.ExProperty
{
    public class Property
    {
        public List<Collision> Collisions { get; set; }
        public List<Decolation> Decolations { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<Item> Items { get; set; }
        public Player PlayerInfo { get; set; }
        public Stage StageInfo { get; set; }

        public Property()
        {
            Collisions = new List<Object.ExProperty.Collision>();
            Decolations = new List<Object.ExProperty.Decolation>();
            Enemies = new List<Object.ExProperty.Enemy>();
            Items = new List<Object.ExProperty.Item>();
            PlayerInfo = new Object.ExProperty.Player();
            StageInfo = new Object.ExProperty.Stage();
        }

        public Property(StageRW.Objects objects)
        {
            Collisions = new List<Object.ExProperty.Collision>();
            Decolations = new List<Object.ExProperty.Decolation>();
            Enemies = new List<Object.ExProperty.Enemy>();
            Items = new List<Object.ExProperty.Item>();
            PlayerInfo = new Object.ExProperty.Player();
            StageInfo = new Object.ExProperty.Stage();

            PlayerInfo.PlayerInfo = objects.Player;
            StageInfo.StageInfo = objects.Stage;

            foreach (var item in objects.Collisions)
            {
                Collisions.Add(new Collision()
                {
                    CollisionInfo = item,
                    Line = new Line()
                });
            }

            foreach (var item in objects.Decolations)
            {
                Decolations.Add(new Decolation()
                {
                    DecolationInfo = item
                });
            }
            foreach (var item in objects.Enemies)
            {
                Enemies.Add(new Enemy()
                {
                    EnemyInfo = item
                });
            }
            foreach (var item in objects.Items)
            {
                Items.Add(new Item()
                {
                    ItemInfo = item
                });
            }
        }

        public StageRW.Objects ToStructObjects()
        {
            StageRW.Objects new_obj = new StageRW.Objects()
            {
                Collisions = new List<StageRW.Property.Collision>(),
                Decolations = new List<StageRW.Property.Decolation>(),
                Enemies = new List<StageRW.Property.Enemy>(),
                Items = new List<StageRW.Property.Item>(),
                Player = new StageRW.Property.Player(),
                Stage = new StageRW.Property.Stage()
            };

            foreach (var item in Collisions)
            {
                new_obj.Collisions.Add(item.CollisionInfo);
            }

            foreach (var item in Decolations)
            {
                new_obj.Decolations.Add(item.DecolationInfo);
            }

            foreach (var item in Enemies)
            {
                new_obj.Enemies.Add(item.EnemyInfo);
            }

            foreach (var item in Items)
            {
                new_obj.Items.Add(item.ItemInfo);
            }

            new_obj.Player = PlayerInfo.PlayerInfo;
            new_obj.Stage = StageInfo.StageInfo;

            return new_obj;
        }
    }
}
