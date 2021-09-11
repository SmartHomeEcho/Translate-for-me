using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Fordító
{
    public partial class Form1 : Form
    {
        List<string> NyelvekLista = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }
        private IRestResponse KeresSzolgaltatas(string UrlCim)
        {
            //Kliens példányosítása, létrehozása, AlkalmazasCache adatainak meghívása
            var Kliens = new RestClient()
            {

                BaseUrl = new Uri(UrlCim)
            };
            //Keresés metódus példányosítása
            var Keres = new RestRequest()
            {
                Method = Method.GET
            };
            //Kliensen a keresés lefuttatása, amely a választ eredményezi
            return Kliens.Execute(Keres);
        }
       
        //Szokásos bezárás kérdéssel
        private void Bezaras_Gomb_Click(object sender, EventArgs e)
        {
            {
                if (MessageBox.Show("Biztosan ki szeretne lépni?", "Biztosan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    try
                    {
                        Application.Exit();
                    }
                    catch
                    {
                        return;
                    }
                }
            }
        }
        //
        private void NyelvFelismereseGomb_Click(object sender, EventArgs e)
        {

            var Valasz= KeresSzolgaltatas(string.Format(AlkalmazasCache.ForrasNyelvDetektalas, AlkalmazasCache.API, ForditandoTextBox.Text));
             //A válasz kimentése egy Szótárba (IDictionary)
             var Szotar= JsonConvert.DeserializeObject<IDictionary>(Valasz.Content);
            //Statuszkód változó ha a szótár "code" értéke 200
            var StatuszKod = Szotar["code"].ToString();
            //Ha az érték 200 a labelbe kiírja a felismert nyelvet.
            if (StatuszKod.Equals("200"))
            {
                FelismertNyelvLabel.Text = Szotar["lang"].ToString();
            }
        }

        private void AC_Click(object sender, EventArgs e)
        {
            var Valasz = KeresSzolgaltatas(string.Format(AlkalmazasCache.LehetsegesNyelvek, AlkalmazasCache.API, FelismertNyelvLabel.Text));
            var Szotar = JsonConvert.DeserializeObject<IDictionary>(Valasz.Content);
            foreach (DictionaryEntry entry in Szotar)
            {
                if (entry.Key.Equals("langs"))
                {
                    var LehetsegesKonvertalas = (JObject)entry.Value;
                    NyelvekLista = new List<string>();
                    NyelvValaszto.Items.Clear();
                    foreach (var Lang in LehetsegesKonvertalas)
                    {
                        if (!Lang.Equals(FelismertNyelvLabel.Text))
                        {
                            NyelvValaszto.Items.Add(Lang.Value);
                            NyelvekLista.Add(Lang.Key);
                        }
                    }
                }
            }
        }

       

        private void Fordítás_Gomb_Click(object sender, EventArgs e)
        {
            var Valasz = KeresSzolgaltatas(string.Format(AlkalmazasCache.NyelvForditas, AlkalmazasCache.API, ForditandoTextBox.Text, NyelvekLista[NyelvValaszto.SelectedIndex] ));
            var Szotar = JsonConvert.DeserializeObject<IDictionary>(Valasz.Content);
            var StatuszKod = Szotar["code"].ToString();
            if (StatuszKod.Equals("200"))
            {
               LeforditottSzoveg.Text= string.Join(",", Szotar["text"]);
            }
        }

        private void InformacioGomb_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. Legfelső szövegdobozban azz meg a fordítandó szöveget\n" +
                "2. Nyelv felismerése gombra kattintva kiírja a bal alsó sarokban a detektált nyelvet\n" +
                "3. Ezután Lehetséges nyelvek feltöltése gombra kell kattintani, amely következtében az alatta lévő lenyíló listát feltölti\n" +
                "4. Listábol válaszd ki a kívánt nyelvet, majd fordítás gomb!:).\n"
                , "Információ (ha nem tudod használni kérdezz!)", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
