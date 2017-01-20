using Photography.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photography.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Accessory> Accessories { get; }
        IRepository<Camera> Cameras { get; }
        IRepository<Len> Lens { get; }
        IRepository<Photographer> Photographers { get; }
        IRepository<Workshop> Workshops { get; }
        
        void Commit();
    }
}
