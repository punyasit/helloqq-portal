using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Models;
using HelloQQPortal.Database;
using System.Data;
using System.Data.Entity;

namespace HelloQQPortal.Manager
{
    public class MemberManager
    {
        private helloqqdbEntities dbInfo = new helloqqdbEntities();

        public MemberManager()
        {

        }

        public List<member> GetMemberList()
        {
            List<member> lstMember = dbInfo.members.ToList();
            return dbInfo.members.ToList();
        }

        public member GetMemberById(int id)
        {
            member member = dbInfo.members.Find(id);
            return member;
        }

        public member Login(UserInfo userInfo)
        {
            member result;
            using (dbInfo = new helloqqdbEntities())
            {
                result = dbInfo.members
                    .Where(item => item.facebook_id == userInfo.id)
                    .FirstOrDefault();

                if(result != null)
                {
                    log_login entityInfo = new log_login();
                    entityInfo.login_time = DateTime.Now;
                    entityInfo.member_id = result.id;

                    if(result.picture_url != userInfo.photoURL)
                    {
                        result.picture_url = userInfo.photoURL;
                        result.modified_on = DateTime.Now;
                    }

                    if (result.facebook_name != userInfo.name)
                    {
                        result.facebook_name = userInfo.name;
                        result.modified_on = DateTime.Now;
                    }

                    dbInfo.log_login.Add(entityInfo);
                    dbInfo.SaveChanges();
                }
                
            }

            return result;
        }

        public member UpdateMember(member memberInfo)
        {
            member result;
            using (dbInfo = new helloqqdbEntities())
            {
                result = dbInfo.members
                    .Where(item => item.id == memberInfo.id)
                    .FirstOrDefault();

                if(result != null)
                {
                    result.fullname = memberInfo.fullname;
                    result.address = memberInfo.address;
                    result.modified_on = DateTime.Now;
                    result.role = memberInfo.role;
                    result.status = memberInfo.status;

                    dbInfo.SaveChanges();
                }
            }

             return memberInfo;
        }
    }
}