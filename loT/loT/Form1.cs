using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;



namespace loT
{
    public partial class Form1 : Form
    {
        GMapOverlay overlay = new GMapOverlay("markers");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            overlay.Markers.Clear();
            string jsonString;
            var url = "https://dev.rightech.io/api/v1/objects/61a0ed796b93f500107164f5";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Accept = "application/json";
            httpRequest.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI2MTlmYjNiNmM0M2U0MDAwMTAzZjNjYjkiLCJzdWIiOiI2MTM5MTJlZjVkYWIzNzAwMTA1YjM5MzAiLCJncnAiOiI2MTM5MTJlZjVkYWIzNzAwMTA1YjM5MmYiLCJvcmciOiI2MTM5MTJlZjVkYWIzNzAwMTA1YjM5MmYiLCJsaWMiOiI1ZDNiNWZmMDBhMGE3ZjMwYjY5NWFmZTMiLCJ1c2ciOiJhcGkiLCJmdWxsIjpmYWxzZSwicmlnaHRzIjoxLjUsImlhdCI6MTYzNzg1NjE4MiwiZXhwIjoxNjQwMzc5NjAwfQ.mr80NaseFvh92e4ADz4OWes8xgJXzRFWY56xVM4BXfM";

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                jsonString = streamReader.ReadToEnd();
            }
            Data data = new Data();
            var state = System.Text.Json.JsonSerializer.Deserialize<dynamic>(jsonString).GetProperty("state");
            data.fuel = Convert.ToInt32(state.GetProperty("fuel").GetDouble());
            data.mileage = Convert.ToInt32(state.GetProperty("mileage").GetDouble());
            data.speed = Convert.ToInt32(state.GetProperty("speed").GetDouble());
            data.door_position = state.GetProperty("door_position").GetBoolean();
            data.headlight = state.GetProperty("headlight").GetBoolean();
            data.latitude = state.GetProperty("lat").GetDouble();
            data.longitude = state.GetProperty("lon").GetDouble();


            label8.Text = data.latitude.ToString();
            label9.Text = data.longitude.ToString();
            label10.Text = data.speed.ToString();
            label11.Text = data.fuel.ToString();
            label12.Text = data.mileage.ToString();
            label13.Text = data.door_position.ToString();
            label14.Text = data.headlight.ToString();

            GMarkerGoogle mapMarker = new GMarkerGoogle(new GMap.NET.PointLatLng(data.latitude, data.longitude), GMarkerGoogleType.red);
            overlay.Markers.Add(mapMarker);
            gMapControl1.Overlays.Add(overlay);
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache; //выбор подгрузки карты – онлайн или из ресурсов
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance; //какой провайдер карт используется (в нашем случае гугл) 
            gMapControl1.MinZoom = 2; //минимальный зум
            gMapControl1.MaxZoom = 16; //максимальный зум
            gMapControl1.Zoom = 10; // какой используется зум при открытии
            gMapControl1.Position = new GMap.NET.PointLatLng(55.755829, 37.617627);// точка в центре карты при открытии (центр России)
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter; // как приближает (просто в центр карты или по положению мыши)
            gMapControl1.CanDragMap = true; // перетаскивание карты мышью
            gMapControl1.DragButton = MouseButtons.Left; // какой кнопкой осуществляется перетаскивание
            gMapControl1.ShowCenter = false; //показывать или скрывать красный крестик в центре
            gMapControl1.ShowTileGridLines = false; //показывать или скрывать тайлы
        }
    }
}
