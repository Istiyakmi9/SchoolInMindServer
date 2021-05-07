using BottomhalfCore.BottomhalfModel;
using BottomhalfCore.ContextFactoryManager.Interface;
using BottomhalfCore.Exceptions;
using BottomhalfCore.IFactoryContext;
using IFactoryContext.IFactoryContext;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BottomhalfCore.FactoryContext
{
    public class ResolverClassType
    {
        public IContainer container = null;
        private List<string> WhiteList = null;
        public string Bindir = null;
        private Boolean DocumentGenerationFlag = false;

        /// <summary>ResolverClassType Constructor
        /// <para></para>
        /// </summary>
        public ResolverClassType()
        {
            List<AopDetail> aopDetailLst;
            IFileCollector fileCollector = null;
            try
            {
                container = Container.GetInstance();
                WhiteList = new List<string>();
                aopDetailLst = new List<AopDetail>();
                string CurrentProjectDirectory = this.container.GetProjectPath();

                #region FILECOLLECTOR

                /// <summary>FileCollector read files
                /// <para></para>
                /// </summary>

                fileCollector = new FileCollector();
                if (string.IsNullOrEmpty(CurrentProjectDirectory))
                    fileCollector.DiscoverPath();

                //if (container.ProjectNamesIsNotEmpty())
                //    fileCollector.ReadCoreProjectFile();

                #endregion

                SetUpContextEnvironment();
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "ResolverClassType()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "ResolverClassType()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }

        /// <summary>SetUpContextEnvironment
        /// <para></para>
        /// </summary>
        private void SetUpContextEnvironment()
        {
            ILoadAsmTypes<LoadAsmTypes> loadAsmTypes;
            IOrganizeContainer organizeContainer = null;
            IDictionary<string, TypeRefCollection> beansType = null;
            ConcurrentDictionary<string, GraphContainerModal> containerCollection = null;
            List<DocCollector> docCollectorlst = null;
            try
            {
                loadAsmTypes = new LoadAsmTypes();
                organizeContainer = new OrganizeContainer();
                //var ProjectNames = container.GetProjectName();
                beansType = loadAsmTypes.DiscoverClassFiles();
                container.ContainerStatus(true);

                containerCollection = organizeContainer.ReOrganizeContainer(beansType);
                if (containerCollection != null && containerCollection.Count() > 0)
                {
                    container.SetGraphContainerModalCollection(containerCollection);
                    if (DocumentGenerationFlag && docCollectorlst != null && docCollectorlst.Count() > 0)
                        container.SetProjectDocumentation(docCollectorlst);
                }
                else
                {
                    BeanException ObjBeanException = new BeanException();
                    ObjBeanException.SetMessage("Not able to create Container context.");
                    throw ObjBeanException;
                }
            }
            catch (BeanException _beanEx)
            {
                _beanEx.LocationTrack(this.GetType().FullName + "SetUpContextEnvironment()");
                throw _beanEx;
            }
            catch (Exception ex)
            {
                BeanException ObjBeanException = new BeanException();
                ObjBeanException.LocationTrack(this.GetType().FullName + "SetUpContextEnvironment()");
                ObjBeanException.SetMessage(ex.Message);
                throw ObjBeanException;
            }
        }
    }
}
