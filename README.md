# Лабораторная работа 14.09.2026

## Что это за проект

Учебный проект по C# на .NET 8.0. Работа с JSON-файлами через Web API.
Проект состоит из трёх частей:

- AsyncJsonModule - библиотека классов, там лежат модели, интерфейсы и репозитории
- AsyncJsonHomework - консольное приложение для проверки работы репозиториев
- AsyncJsonWeb - веб-приложение на ASP.NET Core, там контроллеры

## Как устроено

JSON-файлы (users.json, notes.json, events.json) лежат в папке Data в корне решения.

В AsyncJsonModule:
- папка Interfaces - тут интерфейсы репозиториев
- папка Models - тут классы User, Note, Event
- папка Repositories - тут реализации репозиториев

В AsyncJsonWeb в папке Controllers лежат UserController, NoteController, EventController.

Репозитории регистрируются в Program.cs веб-проекта через AddSingleton. Singleton значит что объект создаётся один раз и используется всё время работы программы. Для работы с файлом это подходит, потому что файл один.

## Что было в задании

### Задание 1
Добавить в класс Event свойство int MaxParticipants. Сделал в Models/Event.cs.

### Задание 2
Сделать метод который возвращает только будущие события. Адрес: GET /api/events/future.
Сделал в EventController и EventJsonRepository. Будущие события определяю по date > DateTime.Now.

### Задание 3
Сделать поиск событий по названию. Адрес: GET /api/events/search/{name}.
Тоже в контроллере и репозитории. Ищу через Contains без учёта регистра.

### Задание 4
Проверка данных при создании события. Проверяю:
- название не пустое
- описание не пустое
- дата не в прошлом
- MaxParticipants больше 0

Проверка сделана в EventJsonRepository и в EventController.

## Как проверял

Всё проверял через Swagger.

- Добавление корректного события прошло с кодом 200
- Добавление события с пустым названием, старой датой и MaxParticipants = 0 вернуло 400
- Метод future вернул только будущее событие
- Метод search нашёл событие по слову "Конференция"

## Прошлое задание

В NoteController и UserController есть заглушки для методов GetNotesByOwnerIdAsync и DeleteNotesByOwnerIdAsync. Реализацию добавлю когда будет база данных.