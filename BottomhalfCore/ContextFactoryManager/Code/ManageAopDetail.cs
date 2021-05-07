using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.ContextFactoryManager.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BottomhalfCore.Exceptions;

namespace BottomhalfCore.ContextFactoryManager.Code
{
    public class ManageAopDetail : IManageAopDetail<ManageAopDetail>
    {
        /// <summary>FindAopOnClass
        /// <para></para>
        /// </summary>
        public void FindAopOnClass(TypeRefCollection ClassDetail, List<AopDetail> aopDetailLst)
        {
            try
            {
                if (aopDetailLst != null && aopDetailLst.Count() > 0)
                {
                    string Name = ClassDetail.ClassFullyQualifiedName.Split(new char[] { '`' })[0];
                    var AOPClassDetail = aopDetailLst.Where(x => x.ForNameSpace == Name).FirstOrDefault();
                    if (AOPClassDetail != null)
                    {
                        ClassDetail.IsAOPEnabled = true;
                        ClassDetail.AppliedAopDetail = AOPClassDetail;
                    }
                }
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "EnableAOP()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "EnableAOP()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }
    }
}
