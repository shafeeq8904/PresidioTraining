﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cardiologist.Exceptions;
using cardiologist.Models;

namespace cardiologist.Repositories
{
    public class AppointmentRepository : Repository<int, Appointment>
    {
        public override ICollection<Appointment> GetAll()
        {
            if (_items.Count == 0)
            {
                throw new CollectionEmptyException("No appointments found.");
            }
            return _items;
        }

        public override Appointment GetById(int id)
        {
            var appointment = _items.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }
            return appointment;
        }

        protected override int GenerateID()
        {
            if (_items.Count == 0)
                return 101;
            return _items.Max(a => a.Id) + 1;
        }
    }
}
