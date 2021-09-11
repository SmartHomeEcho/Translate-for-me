namespace Fordító
{
    class AlkalmazasCache
    {
        //Trial API kulcs és minden más is csak olvasható!!
        public static string API { get; } = @"trnsl.1.1.20210911T104318Z.643311325218e5f6.84ea491c36faa404a5c344fe4f96b9e77ad3e47e";
        //Nyelvek lekérése a yandexről Postmannel érdemes tesztelni a működését.
        //1. Postman--->get: bemásolni a címet, hogy a megfelelő kulcs és egy tetszőleges nyelvet beírsz. Ezzel lehet tesztelni a lekérés működését
        //2. https://translate.yandex.net/api/v1.5/tr.json/getLangs?key=trnsl.1.1.20210911T104318Z.643311325218e5f6.84ea491c36faa404a5c344fe4f96b9e77ad3e47e&ui=hu
        //3. az alábbi mindhárom esetben így lehet tesztelni a lekérdezéseket, csak be kell helyettesíteni.
        public static string LehetsegesNyelvek { get; } = @"https://translate.yandex.net/api/v1.5/tr.json/getLangs?key={0}&ui={1}";
        public static string ForrasNyelvDetektalas { get; } = @"https://translate.yandex.net/api/v1.5/tr.json/detect?key={0}&text={1}";
        public static string  NyelvForditas { get; } = @"https://translate.yandex.net/api/v1.5/tr.json/translate?key={0}&text={1}&lang={2}";
    }
    
}
