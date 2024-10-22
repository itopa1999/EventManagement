using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;

namespace backend.Services
{
    public class StateLgaService
    {
        private readonly List<StateDto> _states = new List<StateDto>
        {
            new StateDto
            {
                Id= 1,
                Name= "Abia",
                LGAs = new List<string>
                
                {
                    "Aba North",
                    "Aba South",
                    "Arochukwu",
                    "Bende",
                    "Ikwuano",
                    "Isiala Ngwa North",
                    "Isiala Ngwa South",
                    "Isuikwuato",
                    "Obi Ngwa",
                    "Ohafia",
                    "Osisioma Ngwa",
                    "Ugwunagbo",
                    "Ukwa East",
                    "Ukwa West",
                    "Umuahia North",
                    "Umuahia South",
                    "Umu Nneochi"
            
                }
            },
            new StateDto
            {
                Id = 2,
                Name = "Adamawa",
                LGAs = new List<string>
                {
                    "Demsa",
                    "Fufore",
                    "Ganye",
                    "Gayuk",
                    "Gombi",
                    "Grie",
                    "Hong",
                    "Jada",
                    "Lamurde",
                    "Madagali",
                    "Maiha",
                    "Mayo Belwa",
                    "Michika",
                    "Mubi North",
                    "Mubi South",
                    "Numan",
                    "Shelleng",
                    "Song",
                    "Toungo",
                    "Yola North",
                    "Yola South"
            }
            },
            new StateDto
            {
                Id = 3,
                Name = "Akwa Ibom",
                LGAs = new List<string>
                {
                    "Abak",
                    "Eastern Obolo",
                    "Eket",
                    "Esit Eket",
                    "Essien Udim",
                    "Etim Ekpo",
                    "Etinan",
                    "Ibeno",
                    "Ibesikpo Asutan",
                    "Ibiono-Ibom",
                    "Ika",
                    "Ikono",
                    "Ikot Abasi",
                    "Ikot Ekpene",
                    "Ini",
                    "Itu",
                    "Mbo",
                    "Mkpat-Enin",
                    "Nsit-Atai",
                    "Nsit-Ibom",
                    "Nsit-Ubium",
                    "Obot Akara",
                    "Okobo",
                    "Onna",
                    "Oron",
                    "Oruk Anam",
                    "Udung-Uko",
                    "Ukanafun",
                    "Uruan",
                    "Urue-Offong/Oruko",
                    "Uyo"
            }
            },
            new StateDto
            {
                Id =4,
                Name = "Anambra",
                LGAs = new List<string>
                {
                    "Aguata",
                    "Anambra East",
                    "Anambra West",
                    "Anaocha",
                    "Awka North",
                    "Awka South",
                    "Ayamelum",
                    "Dunukofia",
                    "Ekwusigo",
                    "Idemili North",
                    "Idemili South",
                    "Ihiala",
                    "Njikoka",
                    "Nnewi North",
                    "Nnewi South",
                    "Ogbaru",
                    "Onitsha North",
                    "Onitsha South",
                    "Orumba North",
                    "Orumba South",
                    "Oyi"
            }
            },
            new StateDto
            {
                Id =5,
                Name = "Bauchi",
                LGAs = new List<string>
                {
                    "Alkaleri",
                    "Bauchi",
                    "Bogoro",
                    "Damban",
                    "Darazo",
                    "Dass",
                    "Gamawa",
                    "Ganjuwa",
                    "Giade",
                    "Itas/Gadau",
                    "Jama'are",
                    "Katagum",
                    "Kirfi",
                    "Misau",
                    "Ningi",
                    "Shira",
                    "Tafawa Balewa",
                    "Toro",
                    "Warji",
                    "Zaki"
            }
            },
            new StateDto
            {
                Id =6,
                Name = "Bayelsa",
                LGAs = new List<string>
                {
                    "Brass",
                    "Ekeremor",
                    "Kolokuma/Opokuma",
                    "Nembe",
                    "Ogbia",
                    "Sagbama",
                    "Southern Ijaw",
                    "Yenagoa"
            }
            },
            new StateDto
            {
                Id =7,
                Name = "Benue",
                LGAs = new List<string>
                {
                    "Ado",
                    "Agatu",
                    "Apa",
                    "Buruku",
                    "Gboko",
                    "Guma",
                    "Gwer East",
                    "Gwer West",
                    "Katsina-Ala",
                    "Konshisha",
                    "Kwande",
                    "Logo",
                    "Makurdi",
                    "Obi",
                    "Ogbadibo",
                    "Ohimini",
                    "Oju",
                    "Okpokwu",
                    "Otukpo",
                    "Tarka",
                    "Ukum",
                    "Ushongo",
                    "Vandeikya"
            }
            },
            new StateDto
            {
                Id =8,
                Name = "Borno",
                LGAs = new List<string>
                {
                    "Abadam",
                    "Askira/Uba",
                    "Bama",
                    "Bayo",
                    "Biu",
                    "Chibok",
                    "Damboa",
                    "Dikwa",
                    "Gubio",
                    "Guzamala",
                    "Gwoza",
                    "Hawul",
                    "Jere",
                    "Kaga",
                    "Kala/Balge",
                    "Konduga",
                    "Kukawa",
                    "Kwaya Kusar",
                    "Mafa",
                    "Magumeri",
                    "Maiduguri",
                    "Marte",
                    "Mobbar",
                    "Monguno",
                    "Ngala",
                    "Nganzai",
                    "Shani"
            }
            },
            new StateDto
            {
                Id =9,
                Name = "Cross River",
                LGAs = new List<string>
                {
                    "Abi",
                    "Akamkpa",
                    "Akpabuyo",
                    "Bakassi",
                    "Bekwarra",
                    "Biase",
                    "Boki",
                    "Calabar Municipal",
                    "Calabar South",
                    "Etung",
                    "Ikom",
                    "Obanliku",
                    "Obubra",
                    "Obudu",
                    "Odukpani",
                    "Ogoja",
                    "Yakuur",
                    "Yala"
            }
            },
            new StateDto
            {
                Id =10,
                Name = "Delta",
                LGAs = new List<string>
                {
                    "Aniocha North",
                    "Aniocha South",
                    "Bomadi",
                    "Burutu",
                    "Ethiope East",
                    "Ethiope West",
                    "Ika North East",
                    "Ika South",
                    "Isoko North",
                    "Isoko South",
                    "Ndokwa East",
                    "Ndokwa West",
                    "Okpe",
                    "Oshimili North",
                    "Oshimili South",
                    "Patani",
                    "Sapele",
                    "Udu",
                    "Ughelli North",
                    "Ughelli South",
                    "Ukwuani",
                    "Uvwie",
                    "Warri North",
                    "Warri South",
                    "Warri South West"
            }
            },
            new StateDto
            {
                Id =11,
                Name = "Ebonyi",
                LGAs = new List<string>
                {
                    "Abakaliki",
                    "Afikpo North",
                    "Afikpo South",
                    "Ebonyi",
                    "Ezza North",
                    "Ezza South",
                    "Ikwo",
                    "Ishielu",
                    "Ivo",
                    "Izzi",
                    "Ohaozara",
                    "Ohaukwu",
                    "Onicha"
            }
            },
            new StateDto
            {
                Id =12,
                Name = "Edo",
                LGAs = new List<string>
                {
                    "Akoko-Edo",
                    "Egor",
                    "Esan Central",
                    "Esan North-East",
                    "Esan South-East",
                    "Esan West",
                    "Etsako Central",
                    "Etsako East",
                    "Etsako West",
                    "Igueben",
                    "Ikpoba-Okha",
                    "Oredo",
                    "Orhionmwon",
                    "Ovia North-East",
                    "Ovia South-West",
                    "Owan East",
                    "Owan West",
                    "Uhunmwonde"
            }
            },
            new StateDto
            {
                Id =13,
                Name = "Ekiti",
                LGAs = new List<string>
                {
                    "Ado Ekiti",
                    "Efon",
                    "Ekiti East",
                    "Ekiti South-West",
                    "Ekiti West",
                    "Emure",
                    "Gbonyin",
                    "Ido Osi",
                    "Ijero",
                    "Ikere",
                    "Ikole",
                    "Ilejemeje",
                    "Irepodun/Ifelodun",
                    "Ise/Orun",
                    "Moba",
                    "Oye"
            }
            },
            new StateDto
            {
                Id =14,
            Name = "Enugu",
            LGAs = new List<string>
                {
                "Aninri",
                "Awgu",
                "Enugu East",
                "Enugu North",
                "Enugu South",
                "Ezeagu",
                "Igbo Etiti",
                "Igbo Eze North",
                "Igbo Eze South",
                "Isi Uzo",
                "Nkanu East",
                "Nkanu West",
                "Nsukka",
                "Oji River",
                "Udenu",
                "Udi",
                "Uzo Uwani"
            }
            },
            new StateDto
            {
                Id =15,
            Name = "Gombe",
            LGAs = new List<string>
                {
                "Akko",
                "Balanga",
                "Billiri",
                "Dukku",
                "Funakaye",
                "Gombe",
                "Kaltungo",
                "Kwami",
                "Nafada",
                "Shongom",
                "Yamaltu/Deba"
            }
            },
            new StateDto
            {
                Id =16,
            Name = "Imo",
            LGAs = new List<string>
                {
                "Aboh Mbaise",
                "Ahiazu Mbaise",
                "Ehime Mbano",
                "Ezinihitte",
                "Ideato North",
                "Ideato South",
                "Ihitte/Uboma",
                "Ikeduru",
                "Isiala Mbano",
                "Isu",
                "Mbaitoli",
                "Ngor Okpala",
                "Njaba",
                "Nkwerre",
                "Nwangele",
                "Obowo",
                "Oguta",
                "Ohaji/Egbema",
                "Okigwe",
                "Onuimo",
                "Orlu",
                "Orsu",
                "Oru East",
                "Oru West",
                "Owerri Municipal",
                "Owerri North",
                "Owerri West"
            }
            },
            new StateDto
            {
                Id =17,
            Name = "Jigawa",
            LGAs = new List<string>
                {
                "Auyo",
                "Babura",
                "Biriniwa",
                "Birnin Kudu",
                "Buji",
                "Dutse",
                "Gagarawa",
                "Garki",
                "Gumel",
                "Guri",
                "Gwaram",
                "Gwiwa",
                "Hadejia",
                "Jahun",
                "Kafin Hausa",
                "Kaugama",
                "Kazaure",
                "Kiri Kasama",
                "Kiyawa",
                "Maigatari",
                "Malam Madori",
                "Miga",
                "Ringim",
                "Roni",
                "Sule Tankarkar",
                "Taura",
                "Yankwashi"
            }
            },
            new StateDto
            {
                Id =18,
            Name = "Kaduna",
            LGAs = new List<string>
                {
                "Birnin Gwari",
                "Chikun",
                "Giwa",
                "Igabi",
                "Ikara",
                "Jaba",
                "Jema'a",
                "Kachia",
                "Kaduna North",
                "Kaduna South",
                "Kagarko",
                "Kajuru",
                "Kaura",
                "Kauru",
                "Kubau",
                "Kudan",
                "Lere",
                "Makarfi",
                "Sabon Gari",
                "Sanga",
                "Soba",
                "Zangon Kataf",
                "Zaria"
            }
            },
            new StateDto
            {
                Id =19,
            Name = "Kano",
            LGAs = new List<string>
                {
                "Ajingi",
                "Albasu",
                "Bagwai",
                "Bebeji",
                "Bichi",
                "Bunkure",
                "Dala",
                "Dambatta",
                "Dawakin Kudu",
                "Dawakin Tofa",
                "Doguwa",
                "Fagge",
                "Gabasawa",
                "Garko",
                "Garun Mallam",
                "Gaya",
                "Gezawa",
                "Gwale",
                "Gwarzo",
                "Kabo",
                "Kano Municipal",
                "Karaye",
                "Kibiya",
                "Kiru",
                "Kumbotso",
                "Kunchi",
                "Kura",
                "Madobi",
                "Makoda",
                "Minjibir",
                "Nasarawa",
                "Rano",
                "Rimin Gado",
                "Rogo",
                "Shanono",
                "Sumaila",
                "Takai",
                "Tarauni",
                "Tofa",
                "Tsanyawa",
                "Tudun Wada",
                "Ungogo",
                "Warawa",
                "Wudil"
            }
            },
            new StateDto
            {
                Id =20,
            Name = "Katsina",
            LGAs = new List<string>
                {
                "Bakori",
                "Batagarawa",
                "Batsari",
                "Baure",
                "Bindawa",
                "Charanchi",
                "Dandume",
                "Danja",
                "Dan Musa",
                "Daura",
                "Dutsi",
                "Dutsin Ma",
                "Faskari",
                "Funtua",
                "Ingawa",
                "Jibia",
                "Kafur",
                "Kaita",
                "Kankara",
                "Kankia",
                "Katsina",
                "Kurfi",
                "Kusada",
                "Mai'Adua",
                "Malumfashi",
                "Mani",
                "Mashi",
                "Matazu",
                "Musawa",
                "Rimi",
                "Sabuwa",
                "Safana",
                "Sandamu",
                "Zango"
            }
            },
            new StateDto
            {
                Id =21,
            Name = "Kebbi",
            LGAs = new List<string>
                {
                "Aleiro",
                "Arewa Dandi",
                "Argungu",
                "Augie",
                "Bagudo",
                "Birnin Kebbi",
                "Bunza",
                "Dandi",
                "Fakai",
                "Gwandu",
                "Jega",
                "Kalgo",
                "Koko/Besse",
                "Maiyama",
                "Ngaski",
                "Sakaba",
                "Shanga",
                "Suru",
                "Wasagu/Danko",
                "Yauri",
                "Zuru"
            }
            },
            new StateDto
            {
                Id =22,
            Name = "Kogi",
            LGAs = new List<string>
                {
                "Adavi",
                "Ajaokuta",
                "Ankpa",
                "Bassa",
                "Dekina",
                "Ibaji",
                "Idah",
                "Igalamela-Odolu",
                "Ijumu",
                "Kabba/Bunu",
                "Kogi",
                "Lokoja",
                "Mopa Muro",
                "Ofu",
                "Ogori/Mangongo",
                "Okehi",
                "Okene",
                "Olamaboro",
                "Omala",
                "Yagba East",
                "Yagba West"
            }
            },
            new StateDto
            {
                Id =23,
            Name = "Kwara",
            LGAs = new List<string>
                {
                "Asa",
                "Baruten",
                "Edu",
                "Ekiti",
                "Ifelodun",
                "Ilorin East",
                "Ilorin South",
                "Ilorin West",
                "Irepodun",
                "Isin",
                "Kaiama",
                "Moro",
                "Offa",
                "Oke Ero",
                "Oyun",
                "Pategi"
            }
            },
            new StateDto
            {
                Id =24,
            Name = "Lagos",
            LGAs = new List<string>
                {
                "Agege",
                "Ajeromi-Ifelodun",
                "Alimosho",
                "Amuwo-Odofin",
                "Apapa",
                "Badagry",
                "Epe",
                "Eti Osa",
                "Ibeju-Lekki",
                "Ifako-Ijaiye",
                "Ikeja",
                "Ikorodu",
                "Kosofe",
                "Lagos Island",
                "Lagos Mainland",
                "Mushin",
                "Ojo",
                "Oshodi-Isolo",
                "Shomolu",
                "Surulere"
            }
            },
            new StateDto
            {
                Id =25,
            Name = "Nasarawa",
            LGAs = new List<string>
                {
                "Akwanga",
                "Awe",
                "Doma",
                "Karu",
                "Keana",
                "Keffi",
                "Kokona",
                "Lafia",
                "Nasarawa",
                "Nasarawa Egon",
                "Obi",
                "Toto",
                "Wamba"
            }
            },
            new StateDto
            {
                Id =26,
            Name = "Niger",
            LGAs = new List<string>
                {
                "Agaie",
                "Agwara",
                "Bida",
                "Borgu",
                "Bosso",
                "Chanchaga",
                "Edati",
                "Gbako",
                "Gurara",
                "Katcha",
                "Kontagora",
                "Lapai",
                "Lavun",
                "Magama",
                "Mariga",
                "Mashegu",
                "Mokwa",
                "Muya",
                "Paikoro",
                "Rafi",
                "Rijau",
                "Shiroro",
                "Suleja",
                "Tafa",
                "Wushishi"
            }
            },
            new StateDto
            {
                Id =27,
            Name = "Ogun",
            LGAs = new List<string>
                {
                "Abeokuta North",
                "Abeokuta South",
                "Ado-Odo/Ota",
                "Egbado North",
                "Egbado South",
                "Ewekoro",
                "Ifo",
                "Ijebu East",
                "Ijebu North",
                "Ijebu North East",
                "Ijebu Ode",
                "Ikenne",
                "Imeko Afon",
                "Ipokia",
                "Obafemi Owode",
                "Odeda",
                "Odogbolu",
                "Ogun Waterside",
                "Remo North",
                "Shagamu"
            }
            },
            new StateDto
            {
                Id =28,
            Name = "Ondo",
            LGAs = new List<string>
                {
                "Akoko North-East",
                "Akoko North-West",
                "Akoko South-West",
                "Akoko South-East",
                "Akure North",
                "Akure South",
                "Ese Odo",
                "Idanre",
                "Ifedore",
                "Ilaje",
                "Ile Oluji/Okeigbo",
                "Irele",
                "Odigbo",
                "Okitipupa",
                "Ondo East",
                "Ondo West",
                "Ose",
                "Owo"
            }
            },
            new StateDto
            {
                Id =29,
            Name = "Osun",
            LGAs = new List<string>
                {
                "Aiyedaade",
                "Aiyedire",
                "Atakumosa East",
                "Atakumosa West",
                "Boluwaduro",
                "Boripe",
                "Ede North",
                "Ede South",
                "Egbedore",
                "Ejigbo",
                "Ife Central",
                "Ife East",
                "Ife North",
                "Ife South",
                "Ifedayo",
                "Ifelodun",
                "Ila",
                "Ilesa East",
                "Ilesa West",
                "Irepodun",
                "Irewole",
                "Isokan",
                "Iwo",
                "Obokun",
                "Odo Otin",
                "Ola Oluwa",
                "Olorunda",
                "Oriade",
                "Orolu",
                "Osogbo"
            }
            },
            new StateDto
            {
                Id =30,
            Name = "Oyo",
            LGAs = new List<string>
                {
                "Afijio",
                "Akinyele",
                "Atiba",
                "Atisbo",
                "Egbeda",
                "Ibadan North",
                "Ibadan North-East",
                "Ibadan North-West",
                "Ibadan South-East",
                "Ibadan South-West",
                "Ibarapa Central",
                "Ibarapa East",
                "Ibarapa North",
                "Ido",
                "Irepo",
                "Iseyin",
                "Itesiwaju",
                "Iwajowa",
                "Kajola",
                "Lagelu",
                "Ogbomosho North",
                "Ogbomosho South",
                "Ogo Oluwa",
                "Olorunsogo",
                "Oluyole",
                "Ona Ara",
                "Orelope",
                "Ori Ire",
                "Oyo East",
                "Oyo West",
                "Saki East",
                "Saki West",
                "Surulere"
            }
            },
            new StateDto
            {
                Id =31,
            Name = "Plateau",
            LGAs = new List<string>
                {
                "Barkin Ladi",
                "Bassa",
                "Bokkos",
                "Jos East",
                "Jos North",
                "Jos South",
                "Kanam",
                "Kanke",
                "Langtang North",
                "Langtang South",
                "Mangu",
                "Mikang",
                "Pankshin",
                "Qua'an Pan",
                "Riyom",
                "Shendam",
                "Wase"
            }
            },
            new StateDto
            {
                Id =32,
            Name = "Rivers",
            LGAs = new List<string>
                {
                "Abua/Odual",
                "Ahoada East",
                "Ahoada West",
                "Akuku-Toru",
                "Andoni",
                "Asari-Toru",
                "Bonny",
                "Degema",
                "Eleme",
                "Emohua",
                "Etche",
                "Gokana",
                "Ikwerre",
                "Khana",
                "Obio/Akpor",
                "Ogba/Egbema/Ndoni",
                "Ogu/Bolo",
                "Okrika",
                "Omuma",
                "Opobo/Nkoro",
                "Oyigbo",
                "Port Harcourt",
                "Tai"
            }
            },
            new StateDto
            {
                Id =33,
            Name = "Sokoto",
            LGAs = new List<string>
                {
                "Binji",
                "Bodinga",
                "Dange Shuni",
                "Gada",
                "Goronyo",
                "Gudu",
                "Gwadabawa",
                "Illela",
                "Isa",
                "Kebbe",
                "Kware",
                "Rabah",
                "Sabon Birni",
                "Shagari",
                "Silame",
                "Sokoto North",
                "Sokoto South",
                "Tambuwal",
                "Tangaza",
                "Tureta",
                "Wamako",
                "Wurno",
                "Yabo"
            }
            },
            new StateDto
            {
                Id =34,
            Name = "Taraba",
            LGAs = new List<string>
                {
                "Ardo Kola",
                "Bali",
                "Donga",
                "Gashaka",
                "Gassol",
                "Ibi",
                "Jalingo",
                "Karim Lamido",
                "Kurmi",
                "Lau",
                "Sardauna",
                "Takum",
                "Ussa",
                "Wukari",
                "Yorro",
                "Zing"
            }
            },
            new StateDto
            {
                Id =35,
            Name = "Yobe",
            LGAs = new List<string>
                {
                "Bade",
                "Bursari",
                "Damaturu",
                "Fika",
                "Fune",
                "Geidam",
                "Gujba",
                "Gulani",
                "Jakusko",
                "Karasuwa",
                "Machina",
                "Nangere",
                "Nguru",
                "Potiskum",
                "Tarmuwa",
                "Yunusari",
                "Yusufari"
            }
            },
            new StateDto
            {
                Id =36,
                Name = "Zamfara",
                LGAs = new List<string>
                {
                "Anka",
                "Bakura",
                "Birnin Magaji/Kiyaw",
                "Bukkuyum",
                "Bungudu",
                "Chafe",
                "Gummi",
                "Gusau",
                "Kaura Namoda",
                "Maradun",
                "Maru",
                "Shinkafi",
                "Talata Mafara",
                "Zurmi"
            }
            },
            new StateDto
            {
                Id =37,
                Name = "Abuja",
                LGAs = new List<string>
                    {
                        "Abaji",
                        "Bwari",
                        "Gwagwalada",
                        "Kuje",
                        "Kwali",
                        "Municipal Area Council"
                    }
            }
        };
        public List<StateDto> GetAllStates()
        {
            return _states.Select(s => new StateDto { Id = s.Id, Name = s.Name }).ToList();
        }

        public List<string> GetLgasByStateId(int stateId)
        {
            var state = _states.FirstOrDefault(s => s.Id == stateId);
            return state?.LGAs ?? new List<string>();
        }
    }
    
}