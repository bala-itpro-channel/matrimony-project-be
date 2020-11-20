
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Matrimony.Backend.Models
{
    public class Profile
    {
        
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(500)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public int Age { get; set; }
    }

    public interface IProfileRepository
    {
        Profile GetProfile(int id);
        IEnumerable<Profile> GetAllProfile();
        Profile Add(Profile profile);
        Profile Update(Profile profile);
        Profile Delete(int id);
    }

    public class MockProfileRepository: IProfileRepository
    {
        private List<Profile> _profileList = null;

        public MockProfileRepository()
        {
            _profileList.AddRange(new Profile[] {
                new Profile { Id = 1, Age = 12, Email = "test@gmail.com", FullName = "Bala", UserName = "bala" },
                new Profile { Id = 2, Age = 22, Email = "test2@gmail.com", FullName = "Bala2", UserName = "bala2" }
            });
        }

        public Profile GetProfile(int id)
        {
            return _profileList.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Profile> GetAllProfile()
        {
            return _profileList;
        }

        public Profile Add(Profile profile)
        {
            _profileList.Add(profile);

            return profile;
        }

        public Profile Update(Profile updatedProfile)
        {
            var profile = _profileList.FirstOrDefault(p => p.Id == updatedProfile.Id);
            profile.Age = updatedProfile.Age;
            profile.Email = updatedProfile.Email;
            profile.FullName = updatedProfile.FullName;
            profile.UserName = updatedProfile.UserName;

            return profile;
        }

        public Profile Delete(int id)
        {
            var profile = this._profileList.FirstOrDefault(p => p.Id == id);
            _profileList.Remove(profile);

            return profile;
        }

    }

    public class SqlProfileRepository : IProfileRepository
    {
        // private List<Profile> _profileList;
        private readonly MatrimonyDBContext _db;

        public SqlProfileRepository(
            MatrimonyDBContext db
        )
        {
            _db = db;
        }

        public Profile GetProfile(int id)
        {
            return _db.Profiles.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Profile> GetAllProfile()
        {
            return _db.Profiles.ToList();
        }

        public Profile Add(Profile profile)
        {
            _db.Profiles.Add(profile);

            _db.SaveChangesAsync();

            return profile;
        }

        public Profile Update(Profile profile)
        {
            _db.Entry(profile).State = EntityState.Modified;

            _db.SaveChangesAsync();

            return profile;
        }

        public Profile Delete(int id)
        {
            var profile = _db.Profiles.FirstOrDefault(p => p.Id == id);

            _db.Entry(profile).State = EntityState.Deleted;

            _db.SaveChangesAsync();

            return profile;
        }

    }
}
