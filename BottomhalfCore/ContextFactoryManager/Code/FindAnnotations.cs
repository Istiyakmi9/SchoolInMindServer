using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.ContextFactoryManager.Interface;
using BottomhalfCore.Exceptions;
using BottomhalfCore.FactoryContext;
using BottomhalfCore.Flags;
using BottomhalfCore.IFactoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BottomhalfCore.ContextFactoryManager.Code
{
    public class FindAnnotations : IFindClassAssets<FindAnnotations>
    {
        private IContainer container;
        public FindAnnotations()
        {
            this.container = Container.GetInstance();
        }


        /// <summary>InspectClassAnnotation
        /// <para></para>
        /// </summary>
        public ClassAnnotationDetail InspectAnnotations(Type CurrentType, TypeRefCollection refCollection)
        {
            ClassAnnotationDetail classAnnotationDetail;
            List<AopDetail> aopDetailLst = null;
            Boolean DocumentGenerationFlag = false;
            List<AnnotationDefination> ObjAnnotationDefinationList = new List<AnnotationDefination>();
            AnnotationDefination ObjAnnotationDefination = null;
            try
            {
                classAnnotationDetail = new ClassAnnotationDetail();
                foreach (var Annotate in CurrentType.CustomAttributes)
                {
                    if (Annotate.AttributeType.Name == Constants.Scoped)
                        refCollection.IsScoped = true;
                    else if (Annotate.AttributeType.Name == Constants.SingleTon)
                        refCollection.IsSingleTon = true;
                    else if (Annotate.AttributeType.Name == Constants.Transient)
                        refCollection.IsTransient = true;
                    else if (Annotate.AttributeType.Name == "GenerateDoc")
                        DocumentGenerationFlag = true;
                    else
                    {
                        ObjAnnotationDefinationList.Add(new AnnotationDefination { AnnotationName = Annotate.AttributeType.Name });
                        if (Annotate.AttributeType.FullName.IndexOf("BottomhalfCore.Annotations") != -1)
                        {
                            if (Annotate.AttributeType.Name == "NoCheck")
                                container.AddNoCheckClass(CurrentType.FullName.ToLower());

                            if (IsAopApplied(Annotate.AttributeType.Name))
                            {
                                string ReturnType = Annotate.ConstructorArguments[0].Value.ToString();
                                string NameSpace = Annotate.ConstructorArguments[1].Value.ToString();
                                string MethodExpression = Annotate.ConstructorArguments[2].Value.ToString();
                                string Arguments = Annotate.ConstructorArguments[3].Value.ToString();
                                if (!string.IsNullOrEmpty(ReturnType) && !string.IsNullOrEmpty(NameSpace) && !string.IsNullOrEmpty(MethodExpression) && !string.IsNullOrEmpty(Arguments))
                                {
                                    AopDetail ObjAopDetail = new AopDetail();
                                    ObjAopDetail.AOPType = Annotate.AttributeType.Name;
                                    ObjAopDetail.ForArgumentType = Arguments;
                                    ObjAopDetail.ForNameSpace = NameSpace;
                                    ObjAopDetail.ForWhichReturnType = ReturnType;
                                    ObjAopDetail.MethodExpression = MethodExpression;
                                    ObjAopDetail.AspectClassName = CurrentType.Name;
                                    ObjAopDetail.AspectFullyQualifiedName = CurrentType.Namespace;
                                    aopDetailLst.Add(ObjAopDetail);
                                }
                            }
                            else
                            {
                                if (ObjAnnotationDefination == null)
                                    ObjAnnotationDefination = new AnnotationDefination();
                                ObjAnnotationDefination.AnnotationName = Annotate.AttributeType.Name;
                                foreach (var CtorArgs in Annotate.ConstructorArguments)
                                {
                                    if (CtorArgs.Value != null)
                                    {
                                        ObjAnnotationDefination.Value.Add(CtorArgs.Value);
                                        ObjAnnotationDefination.AppliedOn = "class";
                                        if (Annotate.AttributeType.Name == "Auth")
                                        {
                                            if (ObjAnnotationDefination.Value.Count > 0)
                                            {
                                                string TokenName = ObjAnnotationDefination.Value.FirstOrDefault();
                                                if (!string.IsNullOrEmpty(TokenName))
                                                    container.SetTokenName(TokenName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                classAnnotationDetail.annotationDefination = ObjAnnotationDefinationList;
                classAnnotationDetail.aopDetailLst = aopDetailLst;
                classAnnotationDetail.IsCodeDocRequired = DocumentGenerationFlag;
                return classAnnotationDetail;
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "InspectClassAnnotation()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "InspectClassAnnotation()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        /// <summary>IsAopApplied
        /// <para></para>
        /// </summary>
        private Boolean IsAopApplied(string Name)
        {
            Boolean Flag = false;
            if (Name == "BeforeAdvice" || Name == "AfterAdvice" || Name == "AroundAdvice" || Name == "ThrowAdvice")
                Flag = true;
            return Flag;
        }

        #region CURRENT INTERFACE MARKUP DECLARATION

        public Dictionary<int, ParameterDetail> InspectMethods(Type CurrentType)
        {
            throw new NotImplementedException();
        }

        public List<FieldAttributes> InspectAttributes(Type CurrentType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
