using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using app_pesquisa.Droid.Utils;
using app_pesquisa.interfaces;
using Android.Net;
using Android.Telephony;
using app_pesquisa.model;
using Android.Preferences;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Android.Graphics;

[assembly: Dependency(typeof(AndroidUtils))]
namespace app_pesquisa.Droid.Utils
{
    public class AndroidUtils : IUtils
    {
		private static Activity mainActivity;

		public static void SetMainActicity(Activity activity)
		{
			mainActivity = activity;
		}
		
        public bool IsOnline()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);

            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;

            NetworkInfo wifiInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi);

            return (wifiInfo != null && wifiInfo.IsConnected) || (activeConnection != null && activeConnection.IsConnected);
        }

        public String ObterIMEI()
        {
            TelephonyManager telephonyManager = (TelephonyManager)Android.App.Application.Context.GetSystemService(Context.TelephonyService);
            String imei = telephonyManager.DeviceId;

            return imei;
        }

        public void UpdatePreference(String key, String tipo, Object valor)
        {
            ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
            ISharedPreferencesEditor editor = preferences.Edit();
            switch (tipo)
            {
                case "Str":
                    editor.PutString(key, (String)valor);
                    break;
                case "Flt":
                    editor.PutFloat(key, (float)valor);
                    break;
                case "Bool":
                    editor.PutBoolean(key, (bool)valor);
                    break;
                case "Int":
                    editor.PutInt(key, (int)valor);
                    break;
                case "Lng":
                    editor.PutLong(key, (long)valor);
                    break;
                default:
                    break;
            }

            editor.Commit();
        }

        public Configuracao ObterConfiguracao()
        {
            ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
            Configuracao conf = new Configuracao();
            conf.EnderecoServidor = preferences.GetString("endereco_servidor", "");
            conf.PercentualMaximoGrafico = preferences.GetFloat("perccentual_maximo_grafico", 0);

            return conf;
        }

        public void SalvarConfiguracao(Configuracao conf)
        {
            UpdatePreference(conf.Key, conf.Tipo, conf.Valor);
        }

        public void InserirConfiguracaoInicial(bool verificarExiste)
        {
            ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
            ISharedPreferencesEditor editor = preferences.Edit();
            if (verificarExiste)
            {
                if (!preferences.Contains("endereco_servidor"))
                {
                    editor.PutString("endereco_servidor", "http://pesquisaam.com/ws_pesquisa/webapi/services/");
                    editor.PutFloat("perccentual_maximo_grafico", 80);
                }
            }
            else
            {
                editor.PutString("endereco_servidor", "http://pesquisaam.com/ws_pesquisa/webapi/services/");
                editor.PutFloat("perccentual_maximo_grafico", 80);
            }

            editor.Commit();
        }

		public void CompartilharCode(String dados)
		{
			if (Android.Support.V4.App.ActivityCompat.CheckSelfPermission(Android.App.Application.Context, Android.Manifest.Permission.WriteExternalStorage) != (int)Android.Content.PM.Permission.Granted)
			{
				Android.Support.V4.App.ActivityCompat.RequestPermissions(mainActivity, new string[] { Android.Manifest.Permission.WriteExternalStorage }, 0);

				return;
			}
			else
			{
				try
				{
					String path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
					String arquivoPDF = System.IO.Path.Combine(path, "code_participante.pdf");

                    FileStream fs = new FileStream(arquivoPDF, FileMode.Create);
					Document document = new Document(PageSize.A4, 25, 25, 30, 30);
					PdfWriter writer = PdfWriter.GetInstance(document, fs);

					document.Open();

                    Paragraph cabecalho = new Paragraph();
                    cabecalho.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                    Phrase lblEvento = new Phrase("Evento XXX \n\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 18f, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK));
                    Phrase lblNome = new Phrase(dados.Split(';')[1] + "\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 15f, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

                    cabecalho.Add(lblEvento);
                    cabecalho.Add(lblNome);

                    document.Add(cabecalho);

                    Paragraph pImg = new Paragraph();
					pImg.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

					MemoryStream ms = new MemoryStream();

                    var bw = new ZXing.Mobile.BarcodeWriter();
					bw.Options = new ZXing.Common.EncodingOptions() { Width = 600, Height = 250, Margin = 0 };
		            bw.Format = ZXing.BarcodeFormat.QR_CODE;
                    bw.Renderer = new ZXing.Mobile.BitmapRenderer();

                    var bitmap = bw.Write(dados);
                    bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 0, ms);
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ms.ToArray());
					img.ScalePercent(60f);
		            img.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

		            pImg.Add(img);

		            document.Add(pImg);

                    document.NewPage();

                    ms.Close();

					document.Close();
                    writer.Close();
                    fs.Close();

					var fileUri = Android.Net.Uri.FromFile(new Java.IO.File(arquivoPDF));
					var sharingIntent = new Intent();
					sharingIntent.SetAction(Intent.ActionSend);
                    sharingIntent.SetType("application/pdf");
                    //sharingIntent.PutExtra(Intent.ExtraText, content);
                    sharingIntent.PutExtra(Intent.ExtraStream, fileUri);
                    //sharingIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    Intent intent = Intent.CreateChooser(sharingIntent, "code_participante.pdf");
					intent.AddFlags(ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(intent);
				}
				catch (Exception ex)
				{
					throw ex;
				}

			}
		}
    }
}