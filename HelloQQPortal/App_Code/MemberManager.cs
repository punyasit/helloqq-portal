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

        public List<hqq_member> GetMemberList()
        {
            List<hqq_member> lsthqq_member = dbInfo.hqq_member
                                            .Include("hqq_member_product")
                                            .ToList();
            return dbInfo.hqq_member.ToList();
        }

        public hqq_member GetMemberById(int id)
        {
            hqq_member hqq_member = dbInfo.hqq_member.Find(id);
            return hqq_member;
        }

        public hqq_member Login(UserInfo userInfo)
        {
            hqq_member result;
            using (dbInfo = new helloqqdbEntities())
            {
                result = dbInfo.hqq_member
                    .Where(item => item.facebook_id == userInfo.id)
                    .FirstOrDefault();

                if (result != null)
                {
                    hqq_log_login entityInfo = new hqq_log_login();
                    entityInfo.login_time = DateTime.Now;
                    entityInfo.member_id = result.id;

                    if (result.picture_url != userInfo.photoURL)
                    {
                        result.picture_url = userInfo.photoURL;
                        result.modified_on = DateTime.Now;
                    }

                    if (result.facebook_name != userInfo.name)
                    {
                        result.facebook_name = userInfo.name;
                        result.modified_on = DateTime.Now;
                    }

                    dbInfo.hqq_log_login.Add(entityInfo);
                    dbInfo.SaveChanges();
                }

            }

            return result;
        }

        public hqq_member UpdateMember(hqq_member hqq_memberInfo)
        {
            hqq_member result;
            using (dbInfo = new helloqqdbEntities())
            {
                result = dbInfo.hqq_member
                    .Where(item => item.id == hqq_memberInfo.id)
                    .FirstOrDefault();

                if (result != null)
                {
                    result.fullname = hqq_memberInfo.fullname;
                    result.address = hqq_memberInfo.address;
                    result.modified_on = DateTime.Now;
                    result.role = hqq_memberInfo.role;
                    result.status = hqq_memberInfo.status;

                    dbInfo.SaveChanges();
                }
            }

            return hqq_memberInfo;
        }

        public hqq_member_product UpdateMemberProduct(hqq_member_product memberProductInfo)
        {
            using (dbInfo = new helloqqdbEntities())
            {
                if (memberProductInfo.id > 0)
                {
                    dbInfo.Entry(memberProductInfo).State = EntityState.Modified;
                    dbInfo.SaveChanges();
                }
                else
                {
                    if (!dbInfo.hqq_member_product.Any(item => item.product_id == memberProductInfo.product_id))
                    {
                        dbInfo.hqq_member_product.Add(memberProductInfo);
                        dbInfo.SaveChanges();
                    }
                }

                dbInfo.SaveChanges();
            }

            return memberProductInfo;
        }

        public bool DeleteMemberProduct(int id)
        {
            bool result = false;
            using (dbInfo = new helloqqdbEntities())
            {

                hqq_member_product selectedItem = dbInfo.hqq_member_product.Find(id);
                if (selectedItem != null)
                {
                    dbInfo.Entry(selectedItem).State = EntityState.Deleted;
                    dbInfo.SaveChanges();

                    result = true;
                }
            }
            return result;
        }
    }
}