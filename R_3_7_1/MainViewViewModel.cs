using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_3_7_1
{
    public class MainViewViewModel
    {
        // Свойства
        private ExternalCommandData _commandData;
        private Document _doc;

        public List<FamilySymbol> TitleBlockType { get; } = new List<FamilySymbol>(null);
        public FamilySymbol SelectedTitleBlockType { get; set; }
        public int TitleQuantity { get; set; }
        public List<View> View { get; } = new List<View>(null);
        public View SelectedView { get; set; }
        public string Designer { get; set; }
        public DelegateCommand SaveCommand { get;}


        // Конструктор
        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _doc = _commandData.Application.ActiveUIDocument.Document;

            TitleBlockType = SheetUtils.GetTitleBlocks(commandData);
            TitleQuantity = 1;
            View = ViewsUtils.GetViews(commandData);
            Designer = "Проектировщик";
            SaveCommand = new DelegateCommand(OnSaveCommand);
        }

        // Методы
        public void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            using(var ts = new Transaction (doc, "Создание листов"))
            {     
                // Отработка исключений
                try
                {
                    ts.Start();

                    // Запускаю цикл for в котором создаю листы с размещенным выбранным видом в центре листа
                    // и заполнением параметра "Разработал". Количество листов задается пользователем в TextBox.
                    for (int i=0; i< TitleQuantity; i++)
                    { 
                    // Создаю переменную типа ViewSheet(Лист) и вызываю метод создания,
                    // которому передаю наш документ b выбранное значение основной надписи
                    // из ComboBox с обозначением его Id/
                    ViewSheet viewSheet = ViewSheet.Create(doc, SelectedTitleBlockType.Id);

                    // Вызываю к переменной метод из библиотеки API(так ли оно?)
                    // get_Parameter SHEET_DESIGNED_BY отвечающий за заполнение поля Разработал в основной надписи
                    // // Set устанавливает параметр на новую строку текста.
                    // Метод Set вернет вернет true если значение параметра было успешно установлено и fals если нет.
                    // Непонятный синтаксис с .Set(Designer)
                    viewSheet.get_Parameter(BuiltInParameter.SHEET_DESIGNED_BY).Set(Designer);

                    // Выполняю проверку. Если наша переменная == нулю то выдает исключение с текстом "Ошибка"
                    if (viewSheet == null)
                    {
                        throw new Exception("Error");
                    }

                    // Создаю переменную (экземпляр класса ElementId, который придает любому элементу данного проекта уникальный Id)
                    // и применяю к этой переменной метод Duplicate
                    // (дублирует данное "представление" - элемент которому мы только что придали уникальный Id).
                    // Этим элементов будет выступать выбранное значение (ВИД View) из ComboBox                    
                    ElementId dublicatedPlanId = SelectedView.Duplicate(ViewDuplicateOption.Duplicate);

                    // Непонятный синтаксис с скобками
                    // Создаю переменную location - экземпляр класса UV
                    // (Объект представляющий координаты в двухмерном пространстве).
                    // В скобках указываю расположение центральной точки как разницу
                    // между наибольшим и наименьшим значением координат деленным на 2 по осям U и V/ 
                    UV location = new UV(viewSheet.Outline.Max.U - viewSheet.Outline.Min.U / 2, 
                                       (viewSheet.Outline.Max.V - viewSheet.Outline.Min.V) / 2);

                    // Создаю переменную viewport - экземпляр класса Viewport
                    // (Элемент, определяющий размещение вида на листе)
                    // и вызываю метод Creat (создания) принадлежащего классу Viewport.
                    // Передаю в метод:
                    // - документ,
                    // - переменную обозначающую лист с выбранной основной надписью,
                    // - переменную обозначающую дублирование (тут не ясно)
                    // - новый экземпляр класса XYZ с координатами (переменными location по осям U и V), что означает 0 я не понимаю. 
                    Viewport viewport = Viewport.Create(doc, viewSheet.Id, dublicatedPlanId, new XYZ(location.U, location.V, 0));
                    }
                    ts.Commit();
                }
                catch
                { }
            }
            RaiseCloseRequest();
        }

        //событие создается для скрытие окна на время выбора.
        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        //событие создается для повторного открытия окна после отработки программы.
        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
        //событие создается для закрытия программы.
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}

