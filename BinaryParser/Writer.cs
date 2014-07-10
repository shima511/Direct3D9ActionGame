using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BinaryParser
{
    public class Writer
    {
        List<byte> byteArray = new List<byte>();

        void AddObjectsSizeInfo(ref Objects objects)
        {
            byteArray.AddRange(BitConverter.GetBytes(objects.Collisions.Count));
            byteArray.AddRange(BitConverter.GetBytes(objects.Items.Count));
            byteArray.AddRange(BitConverter.GetBytes(objects.Decolations.Count));
            byteArray.AddRange(BitConverter.GetBytes(objects.Enemies.Count));
        }

        void AddCheckSum(ref Objects objects)
        {
            int sum = objects.Collisions.Count + objects.Items.Count + objects.Decolations.Count + objects.Enemies.Count;
            byteArray.AddRange(BitConverter.GetBytes(sum));
        }

        void AddPlayerData(ref Objects objects)
        {
            byteArray.AddRange(BitConverter.GetBytes(objects.Player.Position.X));
            byteArray.AddRange(BitConverter.GetBytes(objects.Player.Position.Y));
        }

        void AddStageData(ref Objects objects)
        {
            byteArray.AddRange(BitConverter.GetBytes(objects.Stage.LimitLine.Top));
            byteArray.AddRange(BitConverter.GetBytes(objects.Stage.LimitLine.Right));
            byteArray.AddRange(BitConverter.GetBytes(objects.Stage.LimitLine.Bottom));
            byteArray.AddRange(BitConverter.GetBytes(objects.Stage.LimitLine.Left));
            byteArray.AddRange(BitConverter.GetBytes(objects.Stage.LimitTime));
        }

        void AddCollisionsData(ref Objects objects)
        {
            foreach (var item in objects.Collisions)
            {
                byteArray.AddRange(BitConverter.GetBytes(item.StartingPoint.X));
                byteArray.AddRange(BitConverter.GetBytes(item.StartingPoint.Y));
                byteArray.AddRange(BitConverter.GetBytes(item.TerminatePoint.X));
                byteArray.AddRange(BitConverter.GetBytes(item.TerminatePoint.Y));
                byteArray.AddRange(BitConverter.GetBytes(item.TypeId));
            }
        }

        void AddItemsData(ref Objects objects)
        {
            foreach (var item in objects.Items)
            {
                byteArray.AddRange(BitConverter.GetBytes(item.Position.X));
                byteArray.AddRange(BitConverter.GetBytes(item.Position.Y));
                byteArray.AddRange(BitConverter.GetBytes(item.TypeId));
            }
        }

        void AddDecolationsData(ref Objects objects)
        {
            foreach (var item in objects.Decolations)
            {
                byteArray.AddRange(BitConverter.GetBytes(item.Position.X));
                byteArray.AddRange(BitConverter.GetBytes(item.Position.Y));
                byteArray.AddRange(BitConverter.GetBytes(item.Position.Z));
                byteArray.AddRange(BitConverter.GetBytes(item.TypeId));
            }
        }

        void AddEnemiesData(ref Objects objects)
        {
            foreach (var item in objects.Enemies)
            {
                byteArray.AddRange(BitConverter.GetBytes(item.Position.X));
                byteArray.AddRange(BitConverter.GetBytes(item.Position.Y));
                byteArray.AddRange(BitConverter.GetBytes(item.TypeId));
            }
        }

        public void Write(string filename, Objects objects)
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenRead(filename)))
            {
                AddCheckSum(ref objects);
                AddObjectsSizeInfo(ref objects);
                AddPlayerData(ref objects);
                AddStageData(ref objects);
                AddCollisionsData(ref objects);
                AddItemsData(ref objects);
                AddDecolationsData(ref objects);
                AddEnemiesData(ref objects);
                writer.Write(byteArray.ToArray());
            }
        }
    }
}
