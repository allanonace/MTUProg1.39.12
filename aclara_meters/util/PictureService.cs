using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace aclara_meters.util
{
    public static class PictureService
    {
 
        public static async Task<MediaFile> TakePictureService(string fileName)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("No camera", "No camera available", "OK");
                return null;
            }
            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                // Directory =  user + "_" + Mobile.PATH_IMAGES,//Mobile.ImagesPath,
                Name = fileName,
                SaveToAlbum = false,
                CompressionQuality = 92,
                PhotoSize = PhotoSize.Small,
                DefaultCamera = CameraDevice.Rear

            });
            return file;
        }
    }
}
