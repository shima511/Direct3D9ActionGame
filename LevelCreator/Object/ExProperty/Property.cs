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

        public Property(BinaryParser.Objects objects)
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

        public BinaryParser.Objects ToStructObjects()
        {
            BinaryParser.Objects new_obj = new BinaryParser.Objects()
            {
                Collisions = new List<BinaryParser.Property.Collision>(),
                Decolations = new List<BinaryParser.Property.Decolation>(),
                Enemies = new List<BinaryParser.Property.Enemy>(),
                Items = new List<BinaryParser.Property.Item>(),
                Player = new BinaryParser.Property.Player(),
                Stage = new BinaryParser.Property.Stage()
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
