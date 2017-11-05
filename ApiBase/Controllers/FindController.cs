using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using ApiBase.Entity;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;

namespace ApiBase.Controllers
{
    public class FindController : BaseController
    {
        [AllowAnonymous]
        [Route("Find/SearchService")]
        [HttpPost]
        public SearchServiceResponse SearchService(SearchServiceRqeuest request)
        {
            if (AppSetting("FibbdenFind", "false") == "true")
            {
                throw new BException("禁止访问服务");
            }
            else
            {
                try
                {
                    var ret = new SearchServiceResponse();
                    if (request.Route == "Heart/Heart")
                    {
                        return new SearchServiceResponse()
                        {
                            Attributes = new List<string>(),
                            Exceptions = new List<string>(),
                            Method = "POST",
                            RelativePath = "Heart/Heart",
                            Request = "",
                            Requests = new List<ServiceAttribute>() { },
                            Response = "",
                            Responses = new List<ServiceAttribute>() { },
                            ServcieName = "心跳服务"
                        };
                    }
                    var routes = Configuration.Services.GetApiExplorer().ApiDescriptions.Where(r => r.Route.RouteTemplate == request.Route);
                    #region ex
                    if (routes.Count() == 0)
                    {
                        throw new BException("服务不存在");
                    }
                    if (routes.Count() > 1)
                    {
                        throw new BException("服务存在多个");
                    }
                    #endregion
                    var api = routes.FirstOrDefault();
                    var jObj = new JObject();
                    ret.Attributes = api.ActionDescriptor.GetCustomAttributes<Attribute>(true).Select(r => r.TypeId.ToString()).ToList();
                    ret.Method = api.HttpMethod.Method;
                    ret.RelativePath = api.RelativePath;
                    var serviceXml = api.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly.CodeBase.Replace(".dll", ".xml").Replace(".DLL", ".xml").Replace("file:///", "");
                    #region servicename
                    if (!File.Exists(serviceXml))
                    {
                        throw new BException("服务相关的注释文件不存在!");
                    }
                    else
                    {
                        #region 获取服务名称
                        var file = new FileStream(serviceXml, FileMode.Open, FileAccess.Read);
                        var xmlSearializer = new XmlSerializer(typeof(ApiBase.Entity.Service.doc));
                        var doc1 = (ApiBase.Entity.Service.doc)xmlSearializer.Deserialize(file);
                        file.Close();

                        var methodName = api.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller" + "." + api.ActionDescriptor.ActionName;
                        var i = doc1.members.Where(r => r.name.Contains(methodName + "(")).FirstOrDefault();
                        if (i == null)
                        {
                            i = doc1.members.Where(r => r.name.Contains(methodName)).FirstOrDefault();
                        }
                        if (i != null && i.Items != null && i.Items.Length > 0)
                        {
                            for (int k = 0; k < i.ItemsElementName.Length; k++)
                            {
                                if (i.ItemsElementName[k] == ApiBase.Entity.Service.ItemsChoiceType.summary)
                                {
                                    ret.ServcieName = i.Items[k].ToString().Trim();
                                }
                                else if (i.ItemsElementName[k] == ApiBase.Entity.Service.ItemsChoiceType.exception)
                                {
                                    ret.Exceptions.Add(i.Items[k].ToString().Trim());
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    doc doc = null;
                    #region process request
                    if (api.HttpMethod == HttpMethod.Post)
                    {
                        var parameters = api.ParameterDescriptions;
                        if (parameters.Count == 1)
                        {
                            var pd = parameters[0].ParameterDescriptor;
                            var pdType = pd.ParameterType;
                            #region 解析xml文件
                            var xmlpath = pdType.Assembly.CodeBase.Replace(".dll", ".xml").Replace(".DLL", ".xml").Replace("file:///", "");
                            if (!File.Exists(xmlpath))
                            {
                                throw new BException("请求实体相关的注释文件不存在");
                            }
                            var file = new FileStream(xmlpath, FileMode.Open, FileAccess.Read);
                            var xmlSearializer = new XmlSerializer(typeof(doc));
                            doc = (doc)xmlSearializer.Deserialize(file);
                            file.Close();
                            if (doc == null)
                            {
                                throw new BException("请求实体相关的注释文件不存在!");
                            }
                            #endregion
                            var properties = GetProperties(pd.ParameterType);
                            var i = 0;
                            #region 解析属性
                            for (int j = 0; j < properties.Count; j++)
                            {
                                var propertie = properties[j];
                                if ((propertie.PropertyType.IsGenericType ||
                                         propertie.PropertyType.IsArray) &&
                                     propertie.PropertyType.Name != typeof(Nullable<>).Name)
                                {
                                    jObj.Add(propertie.Name, "[{" + i + "}]");
                                    i = i + 1;
                                    ret.Requests.Add(new ServiceAttribute()
                                    {
                                        Description = GetDesc(doc, propertie.DeclaringType.FullName + "." + propertie.Name, DocT.Property),
                                        Name = propertie.Name,
                                        Type = GetPropertyInfoName(propertie),
                                        IsNullable = GetPropertyIsNullable(propertie, doc, propertie.DeclaringType.FullName + "." + propertie.Name),
                                        ValueDescriptions = GetValueDesc(doc, propertie.DeclaringType.FullName + "." + propertie.Name, DocT.Property),
                                    });
                                }
                                else if (propertie.PropertyType.IsClass &&
                                        propertie.PropertyType != typeof(string))
                                {
                                    var jObjClass = new JObject();
                                    var ps = propertie.PropertyType.GetProperties();
                                    for (int k = 0; k < ps.Length; k++)
                                    {
                                        var psk = ps[k];
                                        if (psk.PropertyType.IsClass &&
                                            psk.PropertyType != typeof(string))
                                        {
                                            jObjClass.Add(psk.Name, "[{" + i + "}]");
                                        }
                                        else
                                        {
                                            jObjClass.Add(psk.Name, "{" + i + "}");
                                        }
                                        i = i + 1;
                                        ret.Requests.Add(new ServiceAttribute()
                                        {
                                            Description = GetDesc(doc, psk.DeclaringType.FullName + "." + psk.Name, DocT.Property),
                                            Name = psk.Name,
                                            Type = GetPropertyInfoName(psk),
                                            IsNullable = GetPropertyIsNullable(psk, doc, psk.DeclaringType.FullName + "." + psk.Name),
                                            ValueDescriptions = GetValueDesc(doc, psk.DeclaringType.FullName + "." + psk.Name, DocT.Property),
                                        });
                                    }
                                    jObj.Add(propertie.Name, jObjClass.ToString(Formatting.None));
                                }
                                else
                                {
                                    jObj.Add(propertie.Name, "{" + i + "}");
                                    i = i + 1;
                                    ret.Requests.Add(new ServiceAttribute()
                                    {
                                        Description = GetDesc(doc, propertie.DeclaringType.FullName + "." + propertie.Name, DocT.Property),
                                        Name = propertie.Name,
                                        Type = GetPropertyInfoName(propertie),
                                        IsNullable = GetPropertyIsNullable(propertie, doc, propertie.DeclaringType.FullName + "." + propertie.Name),
                                        ValueDescriptions = GetValueDesc(doc, propertie.DeclaringType.FullName + "." + propertie.Name, DocT.Property),
                                    });
                                }
                            }
                            #endregion
                        }
                    }
                    ret.Request = (jObj.Count > 0 ? jObj.ToString(Formatting.None) : "").Replace("\"[", "[").Replace("]\"", "]");
                    #endregion
                    #region process response
                    if (api.ResponseDescription.DeclaredType != null)
                    {
                        if (doc == null)
                        {
                            #region 解析xml文件
                            var xmlpath = api.ResponseDescription.DeclaredType.Assembly.CodeBase.Replace(".dll", ".xml").Replace(".DLL", ".xml").Replace("file:///", "");
                            if (!File.Exists(xmlpath))
                            {
                                throw new BException("请求实体相关的注释文件不存在");
                            }
                            var file = new FileStream(xmlpath, FileMode.Open, FileAccess.Read);
                            var xmlSearializer = new XmlSerializer(typeof(doc));
                            doc = (doc)xmlSearializer.Deserialize(file);
                            file.Close();
                            if (doc == null)
                            {
                                throw new BException("请求实体相关的注释文件不存在!");
                            }
                            #endregion
                        }
                        if (api.ResponseDescription.DeclaredType != typeof(String) &&
                            api.ResponseDescription.DeclaredType.IsValueType == false)
                        {
                            ret.Response = JsonConvert.SerializeObject(Activator.CreateInstance(api.ResponseDescription.DeclaredType));
                            var properties1 = GetProperties(api.ResponseDescription.DeclaredType);
                            #region 解析属性
                            for (int j = 0; j < properties1.Count; j++)
                            {
                                var propertie = properties1[j];
                                var s = new ServiceAttribute()
                                {
                                    Description = GetDesc(doc, propertie.DeclaringType.FullName + "." + propertie.Name, DocT.Property),
                                    Name = propertie.Name,
                                    Type = GetPropertyInfoName(propertie),
                                    IsNullable = GetPropertyIsNullable(propertie, doc, propertie.DeclaringType.FullName + "." + propertie.Name),
                                    ValueDescriptions = GetValueDesc(doc, propertie.DeclaringType.FullName + "." + propertie.Name, DocT.Property),
                                };
                                List<PropertyInfo> properties = null;
                                if (propertie.PropertyType.IsGenericType ||
                                        propertie.PropertyType.IsArray)
                                {
                                    if (propertie.PropertyType.GenericTypeArguments != null && propertie.PropertyType.GenericTypeArguments.Length > 0)
                                    {
                                        var type = propertie.PropertyType.GenericTypeArguments[0];
                                        if (type != typeof(String))
                                        {
                                            properties = GetProperties(type);
                                        }
                                    }
                                }
                                else if (propertie.PropertyType.IsClass &&
                                       propertie.PropertyType != typeof(string))
                                {
                                    properties = GetProperties(propertie.PropertyType);
                                }
                                if (properties != null)
                                {
                                    s.ClassDescriptions = new List<ServiceAttribute>();
                                    foreach (var item in properties)
                                    {
                                        s.ClassDescriptions.Add(new ServiceAttribute()
                                        {
                                            Description = GetDesc(doc, item.DeclaringType.FullName + "." + item.Name, DocT.Property),
                                            Name = item.Name,
                                            Type = GetPropertyInfoName(item),
                                            IsNullable = GetPropertyIsNullable(item, doc, item.DeclaringType.FullName + "." + item.Name),
                                            ValueDescriptions = GetValueDesc(doc, item.DeclaringType.FullName + "." + item.Name, DocT.Property),
                                        });
                                    }
                                }
                                ret.Responses.Add(s);
                            }
                            #endregion
                        }
                        else
                        {
                            var apiPath = api.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly.CodeBase.Replace(".dll", ".xml").Replace(".DLL", ".xml").Replace("file:///", "");
                            var file = new FileStream(apiPath, FileMode.Open, FileAccess.Read);
                            var xmlSearializer = new XmlSerializer(typeof(doc));
                            var doc1 = (doc)xmlSearializer.Deserialize(file);
                            file.Close();
                            var path = "Controller." + api.ActionDescriptor.ActionName + "(";
                            var item = doc1.members.FirstOrDefault(r => r.name.Contains(path));
                            if (item != null)
                            {
                                var name = "";
                                if (item.returns != null)
                                {
                                    try
                                    {
                                        name = ((System.Xml.XmlNode[])item.returns).FirstOrDefault().Value;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                ret.Responses.Add(new ServiceAttribute()
                                {
                                    Description = "",
                                    Name = name,
                                    Type = api.ResponseDescription.DeclaredType.Name,
                                    IsNullable = false,
                                    ValueDescriptions = null,
                                });
                            }
                        }
                    }
                    #endregion
                    return ret;
                }
                catch (Exception ex)
                {
                    if (ex is BException)
                    {
                        throw ex;
                    }
                    else
                    {
                        throw new BException(ex.Message + ex.StackTrace);
                    }
                }
            }
        }

        private List<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties().ToList();
        }

        private string GetPropertyInfoName(PropertyInfo p)
        {
            if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments() != null)
            {
                if (p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return p.PropertyType.GetGenericArguments()[0].Name;
                }
                return p.PropertyType.GetGenericArguments()[0].Name + "[]";
            }
            else
            {
                return p.PropertyType.Name;
            }
        }

        private bool GetPropertyIsNullable(PropertyInfo p, doc doc, string name)
        {
            if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return true;
            }
            else
            {
                var d = doc.members.Where(r => r.name == "P:" + name).FirstOrDefault();
                if (d != null && d.@null != null)
                {
                    return true;
                }
                if (d != null && d.summary != null && d.summary.@null != null)
                {
                    return true;
                }
                return false;
            }
        }

        private string GetDesc(doc doc, string name, DocT t)
        {
            if (t == DocT.Property)
            {
                var d = doc.members.Where(r => r.name == "P:" + name).FirstOrDefault();
                if (d != null && d.summary != null && d.summary.Text != null && d.summary.Text.Length > 0)
                {
                    return d.summary.Text[0].Trim(new char[] { ' ', '\n' });
                }
                return "";
            }
            else
            {
                var d = doc.members.Where(r => r.name.StartsWith("T:") && r.name.Contains(name)).FirstOrDefault();
                if (d != null && d.summary != null && d.summary.Text != null && d.summary.Text.Length > 0)
                {
                    return d.summary.Text[0].Trim(new char[] { ' ', '\n' });
                }
                return "";
            }
        }

        private string[] GetValueDesc(doc doc, string name, DocT t)
        {
            if (t == DocT.Property)
            {
                var d = doc.members.Where(r => r.name == "P:" + name).FirstOrDefault();
                if (d != null && d.value != null)
                {
                    return d.value;
                }
                if (d != null && d.summary != null && d.summary.value != null && d.summary.value.Length > 0)
                {
                    return d.summary.value;
                }
                return null;
            }
            else
            {
                var d = doc.members.Where(r => r.name.StartsWith("T:") && r.name.Contains(name)).FirstOrDefault();
                if (d != null && d.value != null)
                {
                    return d.value;
                }
                if (d != null && d.summary != null && d.summary.value != null && d.summary.value.Length > 0)
                {
                    return d.summary.value;
                }
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public enum DocT
        {
            /// <summary>
            /// 
            /// </summary>
            Property,
            /// <summary>
            /// 
            /// </summary>
            Class,
        }
        /// <summary>
        /// 
        /// </summary>
        public sealed class SearchServiceResponse
        {
            /// <summary>
            /// 特性
            /// </summary>
            public List<string> Attributes { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string ServcieName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Method { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string RelativePath { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Request { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Response { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ServiceAttribute> Requests { get; set; }


            /// <summary>
            /// 
            /// </summary>
            public List<ServiceAttribute> Responses { get; set; }


            /// <summary>
            /// 异常信息
            /// </summary>
            public List<string> Exceptions { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public SearchServiceResponse()
            {
                this.Requests = new List<ServiceAttribute>();
                this.Responses = new List<ServiceAttribute>();
                this.Exceptions = new List<string>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public sealed class ServiceAttribute
        {
            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// 是否可空类型
            /// </summary>
            public bool IsNullable { get; set; }

            /// <summary>
            /// 值的描述
            /// </summary>
            public string[] ValueDescriptions { get; set; }

            /// <summary>
            /// 集合中的类描述
            /// </summary>
            public List<ServiceAttribute> ClassDescriptions { get; set; }
        }
    }
}
