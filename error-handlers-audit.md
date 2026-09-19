# Аудит обработчиков ошибок

Дата: 19.09.2026

## Что проверяла

Прошла по всем файлам в AsyncJsonModule и AsyncJsonWeb, посмотрела, где стоит try/catch, что в нём ловится и что делается при ошибке.

Сейчас везде ловится общий Exception ex. Это временно. Позже заменю на конкретные типы ошибок.

## Repositories

### UserJsonRepository.cs

Все методы обёрнуты в try/catch. Ловится Exception ex. В catch пишется сообщение в Console. Если это метод загрузки — возвращается пустой список или new User(). Если метод изменения — возвращается false.

Методы: LoadUsersAsync, GetUserByIdAsync, AddUserAsync, UpdateUserByIdAsync, DeleteUserByIdAsync, SaveUsersAsync.

### NoteJsonRepository.cs

Все методы обёрнуты в try/catch. Ловится Exception ex. В catch пишется сообщение в Console, возвращается пустой список или false.

Методы: LoadNotesAsync, GetNoteByIdAsync, AddNoteAsync, UpdateNoteByIdAsync, DeleteNoteByIdAsync, SaveNotesAsync, GetNotesByOwnerIdAsync, DeleteNotesByOwnerIdAsync.

### EventJsonRepository.cs

Все методы обёрнуты в try/catch. Ловится Exception ex. В catch пишется сообщение в Console, возвращается пустой список или false.

Методы: LoadEventsAsync, GetEventByIdAsync, AddEventAsync, UpdateEventByIdAsync, DeleteEventByIdAsync, SaveEventsAsync, GetFutureEventsAsync, SearchEventsByNameAsync.

## Services

В сервисах try/catch не стоит. Вместо этого идёт проверка данных через if. Если данные некорректные — пишется сообщение и возвращается false.

### UserService.cs

Проверки в AddUserAsync и UpdateUserAsync: email, login, password не пустые, email содержит @.

### NoteService.cs

Проверки в AddNoteAsync: title не пустой, content не пустой, ownerId больше 0.

### EventService.cs

Проверки в AddEventAsync: name не пустой, description не пустой, дата не в прошлом, MaxParticipants больше 0.

## Controllers

В контроллерах try/catch не стоит. Они возвращают коды ответа:
- Ok() — если всё хорошо
- BadRequest() — если данные некорректные
- NotFound() — если объект не найден

Ошибки из репозиториев уже обрабатываются внутри репозиториев, до контроллера доходит уже готовый результат.

## Что планирую улучшить

1. Заменить catch (Exception ex) на конкретные типы: FileNotFoundException, JsonException, IOException, ArgumentException.
2. Заменить Console.WriteLine на ILogger (встроенный в ASP.NET Core).
3. Добавить middleware для отлова необработанных ошибок в веб-проекте, чтобы клиент получал нормальный JSON с ошибкой, а не стек вызовов.е стек вызовов.