﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimDxGame.Object.Decolation
{
    class Field : Object.Base.Model, IFieldObject
    {
        public bool IsActive { get; set; }

        public void Update()
        {

        }

        public void Dispatch(ICollisionObject obj)
        {

        }

        public void Hit(Player player)
        {

        }

        public void Hit(Ground.Floor floor)
        {

        }

        public void Hit(Ground.Ceiling ceiling)
        {

        }

        public void Hit(Ground.RightWall wall)
        {

        }

        public void Hit(Ground.LeftWall wall)
        {

        }
    }
}
