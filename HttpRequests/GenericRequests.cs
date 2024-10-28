using Frontend.Helper;
using Frontend.Models;
using Newtonsoft.Json;
using System.Text;

namespace Frontend.HttpRequests
{
    public class GenericRequests<T>
    {
        public async Task<T> GetHttpRequest(string url)
        {
            var re = Extancion.Client.BaseAddress;
            
            string Url = $"http://{re?.Host}{url}";
            HttpResponseMessage Responce = await Extancion.Client.GetAsync($"{Url}");
            switch (((int)Responce.StatusCode))
            {
                case 401:
                    return default(T);
                //bool again = await refreshTokenDto.Refresh();
                //if (again)
                //{
                //    var data = await GetHttpRequest(url);
                //    return data.ToList();
                //}
                //else
                //{
                //    return new List<T>();
                //};
                case 200:
                    T RsponceApi = await Responce.Content.ReadFromJsonAsync<T>();
                    return RsponceApi;
                default:
                    return default(T);
            }

        }

        public async Task<List<T>> GetListHttpRequest(string url)
        {
            var re = Extancion.Client.BaseAddress;

            string Url = $"http://{re.Host}{url}";
            HttpResponseMessage Responce = await Extancion.Client.GetAsync($"{Url}");
            switch (((int)Responce.StatusCode))
            {
                case 401:
                    return new List<T>();
                //bool again = await refreshTokenDto.Refresh();
                //if (again)
                //{
                //    var data = await GetHttpRequest(url);
                //    return data.ToList();
                //}
                //else
                //{
                //    return new List<T>();
                //};
                case 200:
                    List<T> RsponceApi = await Responce.Content.ReadFromJsonAsync<List<T>>();
                    return RsponceApi;
                default:
                    return new List<T>();
            }

        }

        public async Task<string> PostRequestGeneric(string url, T entity)
        {

            var jsonData = JsonConvert.SerializeObject(entity);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var re = Extancion.Client.BaseAddress;

            string Url = $"http://{re?.Host}{url}";
           
            var data = await Extancion.Client.PostAsync(Url, content);

            switch (((int)data.StatusCode))
            {
                case 200: return "Başarılı";
                case 401:
                    return "Başarısız";
                    //bool again = await refreshTokenDto.Refresh();
                    //if (again)
                    //{
                    //    var agn = await PostRequestGeneric(Url, entity);
                    //    return agn.ToString();
                    //}
                    //else return "Başarısız";
                default: return "Başarısız";
            }
        }
        public async Task<string> UpdateRequestGeneric(string url, T entity)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(entity);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var re = Extancion.Client.BaseAddress;

                string Url = $"http://{re?.Host}{url}";
                var data = await Extancion.Client.PutAsync(Url, content);
                var res = await data.Content.ReadAsStringAsync();
                switch (((int)data.StatusCode))
                {
                    case 200: return "UpdateBaşarılı";
                    case 401:

                        //bool again = await refreshTokenDto.Refresh();
                        //if (again)
                        //{
                        //    var agn = await UpdateRequestGeneric(url, entity);
                        //    return agn.ToString();
                        //}
                        //else
                            return "Başarısız";
                    case 404: return $"404 hatası alındı {data.StatusCode}";
                    default: return $"{data.StatusCode} hata kodu";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<T> GetByIdGeneric(string url, int id)
        {
            var re = Extancion.Client.BaseAddress;

            string Url = $"http://{re?.Host}{url}";
            HttpResponseMessage responce = await Extancion.Client.GetAsync($"{Url}?id={id}");
            switch (((int)responce.StatusCode))
            {
                case 200:
                    T dto = await responce.Content.ReadFromJsonAsync<T>();
                    return dto; ;
                case 401:
                    T obj = new List<T>().First();
                    return obj;
                //bool again = await refreshTokenDto.Refresh();
                //if (again)
                //{
                //    var agn = await GetByIdGeneric(url, id);
                //    return agn;
                //}
                //else
                //{
                //    T obj = new List<T>().First();
                //    return obj;
                //};
                default:
                    T objd = new List<T>().First();
                    return objd; ;
            }
        }

        public async Task<string> DeleteRequestGeneric(string url, int id)
        {
            try
            {
                var re = Extancion.Client.BaseAddress;
                string Url = $"http://{re?.Host}{url}";
                var data = await Extancion.Client.DeleteAsync($"{Url}{id}");
                switch (((int)data.StatusCode))
                {
                    case 200: return "Başarılı";
                    case 401:

                        //bool again = await refreshTokenDto.Refresh();
                        //if (again)
                        //{
                        //    var agn = await DeleteRequestGeneric(url, id);
                        //    return agn.ToString();
                        //}
                        //else
                            return "Başarısız"; ;
                    case 500: return "500 hatası service ulaşılamadı";
                    case 403: return "403 forbidden (yetkisiz rol ile işlem yapmaya çalışmış olabilirsiniz) Servis e ulaşılamadı";
                    case 302: return "302 Hatası Servis e ulaşılamadı";
                    case 404: return $"404 hatası alındı {data.StatusCode}";
                    default: return $"{data.Content.ToString()} , {data.StatusCode} statu kod  ";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
    }
}
