using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using StepCinemaDataLayer;
using StepCinemaModels.Models;

namespace StepCinemaDataLayer.DataAccess
{
    public class UsersLayer : DataAccessBase
    {
        public bool Login(string username, byte[] password)
        {
            var nullValue = new byte[1] { 0x01 };
            return this.Entities.Users.Where(x => x.Email == username && (x.Password ?? nullValue) == (password ?? nullValue)).Any();
            //return this.Entities.Users.Where(x => x.Email == username).Any();
        }

        public bool ChangeMyPassword(string username, byte[] oldpassword, byte[] password)
        {
            var nullValue = new byte[1] { 0x01 };
            var user = this.Entities.Users.Where(x => x.Email == username && (x.Password ?? nullValue) == (oldpassword ?? nullValue)).FirstOrDefault();
            if (user != null)
            {
                user.Password = password;
                this.Entities.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ListPair> GetAvailableRoles()
        {
            return Entities.Roles.Select(x => new ListPair()
            {
                Name = x.RoleName,
                Value = x.RoleId
            }).ToList();
        }

        public UserEditModel GetEditData(int userId)
        {
            UserEditModel model;
            if (userId <= 0)
            {
                model = new UserEditModel();
            }
            else
            {
                model = Entities.Users.Where(x => x.UserId == userId).Select(x => new UserEditModel()
                {
                    UserId = x.UserId,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Active = x.Active
                }).FirstOrDefault();
            }
            if (model != null)
            {
                model.Roles = GetRolesForUser(userId);
                model.AvailableRoles = GetAvailableRoles();
            }
            return model;
        }
        public byte[] GetOldPassword(int userId)
        {
            return Entities.Users.Where(x => x.UserId == userId).Select(x => x.Password).FirstOrDefault();
        }
        public void SaveEditData(UserEditModel model, byte[] md5Password)
        {
            StepCinemaDataLayer.EntityModel.User data;

            if (model.UserId > 0)
            {
                data = Entities.Users.Where(x => x.UserId == model.UserId).FirstOrDefault();
            }
            else
            {
                data = new EntityModel.User();
            }
            if (data != null)
            {
                data.FirstName = model.FirstName;
                data.LastName = model.LastName;
                data.Email = model.Email;
                data.Active = model.Active;
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    data.Password = md5Password;
                }
                if (data.UserId <= 0)
                {
                    Entities.Users.Add(data);
                }
                Entities.SaveChanges();

                var userId = data.UserId;

                var aRoles = this.GetRolesForUser(userId);
                var bRoles = (model.Roles ?? new string[0]);

                var newUserRoles = bRoles.Where(x => !aRoles.Any(y => y == x)).Select(s => new EntityModel.UserRole() { RoleId = s, UserId = userId }).ToList();

                using (var trans = Entities.Database.BeginTransaction())
                {
                    try
                    {
                        Entities.UserRoles.RemoveRange(
                            Entities.UserRoles.Where(x => aRoles.Any(y => y == x.RoleId) && !bRoles.Any(y => y == x.RoleId)).ToList()
                            );
                        if (newUserRoles.Count > 0)
                        {
                            Entities.UserRoles.AddRange(newUserRoles);
                        }
                        Entities.SaveChanges();

                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                    }
                }
            }
        }

        public UserGridModel GetList(UserGridArgumentModel model)
        {
            string firstName = model.Filter.FirstName ?? "";
            string lastName = model.Filter.LastName ?? "";
            string sort = "UserId DESC";
            if ((!string.IsNullOrWhiteSpace(model.Sort.Field)) && (!string.IsNullOrWhiteSpace(model.Sort.Direction)))
            {
                sort = model.Sort.Field + ' ' + model.Sort.Direction;
            }
            else
            {
                model.Sort.Field = "";
                model.Sort.Direction = "";
            }
            var query = this.Entities.Users.OrderBy(sort);
            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(x => (x.FirstName ?? "").Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(x => (x.LastName ?? "").Contains(lastName));
            }
            if (!string.IsNullOrWhiteSpace(model.Filter.Active))
            {
                var active = (model.Filter.Active == "Y");
                query = query.Where(x => x.Active == active);
            }
            var count = query.Count();
            model.Pagination.Count = count;
            model.Pagination.MaxPages = (((count - 1) / model.Pagination.PageSize) + 1);
            if (model.Pagination.CurrentPage > model.Pagination.MaxPages)
            {
                model.Pagination.CurrentPage = model.Pagination.MaxPages;
            }

            var result = query
                .Select(x => new UserGridValueModel()
                {
                    UserId = x.UserId,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Active = ((x.Active) ? "Active" : "Inactive")
                }).Skip((model.Pagination.CurrentPage - 1) * model.Pagination.PageSize).Take(model.Pagination.PageSize).ToList();

            model.Filter.Active = model.Filter.Active ?? "";
            var data = new UserGridModel();
            data.Arguments = model;
            data.Columns = UserGridColumnModel.GetColumns();
            data.Values = result;
            return data;
        }
    }
}
