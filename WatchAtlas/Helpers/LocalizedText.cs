using System.Globalization;
using WatchAtlas.Models.Enums;

namespace WatchAtlas.Helpers;

public static class LocalizedText
{
    private static readonly Dictionary<string, string> Russian = new(StringComparer.Ordinal);

    static LocalizedText()
    {
        AddTranslations(new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["Home"] = "Главная",
            ["Overview"] = "Обзор",
            ["Library"] = "Библиотека",
            ["Statistics"] = "Статистика",
            ["Stats"] = "Статистика",
            ["Settings"] = "Настройки",
            ["Search titles from your library"] = "Искать по вашей библиотеке",
            ["Search titles"] = "Поиск названий",
            ["Add Movie or Series"] = "Добавить фильм или сериал",
            ["Soft tracking for movies and series."] = "Мягкий трекинг фильмов и сериалов.",
            ["Keep movies, series, seasons, and episodes in one calm, easy-to-browse library."] = "Храните фильмы, сериалы, сезоны и эпизоды в одной спокойной и удобной библиотеке.",
            ["Control your app preferences and local data safety."] = "Управляйте настройками приложения и безопасностью локальных данных.",
            ["Choose how the app looks, save a backup, or clear everything and start fresh."] = "Выберите внешний вид приложения, сохраните резервную копию или очистите всё и начните заново.",
            ["Appearance"] = "Внешний вид",
            ["Theme and library defaults"] = "Тема и параметры библиотеки",
            ["These preferences shape how the library opens and how new browsing sessions feel before you start filtering."] = "Эти параметры определяют, как открывается библиотека и как выглядит новый сеанс до применения фильтров.",
            ["Theme"] = "Тема",
            ["Switch live between the soft light, dark, and pastel palettes. The selected theme is preserved after reload."] = "Переключайтесь между мягкой светлой, тёмной и пастельной палитрами. Выбранная тема сохраняется после перезагрузки.",
            ["Light Soft"] = "Мягкая светлая",
            ["Dark Soft"] = "Мягкая тёмная",
            ["Pastel"] = "Пастель",
            ["Warm porcelain surfaces with rosy accents for everyday browsing."] = "Тёплые фарфоровые поверхности с розовыми акцентами.",
            ["Charcoal layers with cool blue highlights and comfortable contrast."] = "Графитовые слои с холодными синими акцентами и комфортным контрастом.",
            ["Lavender and blush surfaces for a softer collector-style library."] = "Лавандовые и румяные поверхности для более мягкой коллекционной библиотеки.",
            ["Rose cloud"] = "Розовое облако",
            ["Moonlit blue"] = "Лунный синий",
            ["Lavender mist"] = "Лавандовый туман",
            ["Theme: {0}"] = "Тема: {0}",
            ["Preferred library view"] = "Предпочитаемый вид библиотеки",
            ["Choose the browsing layout that opens by default on the library page."] = "Выберите режим просмотра, который по умолчанию открывается на странице библиотеки.",
            ["Default library sort"] = "Сортировка библиотеки по умолчанию",
            ["Used when the library opens or when you reset active filters."] = "Используется при открытии библиотеки или при сбросе активных фильтров.",
            ["Grid"] = "Сетка",
            ["List"] = "Список",
            ["Use dense grid cards"] = "Использовать плотные карточки в сетке",
            ["Show a tighter grid layout on the library page when grid view is active."] = "Показывать более плотную сетку на странице библиотеки, когда активен режим сетки.",
            ["Pin completed items first"] = "Показывать завершённые элементы первыми",
            ["Keep completed entries grouped first while preserving the current sort within each group."] = "Держать завершённые записи в начале, сохраняя текущую сортировку внутри каждой группы.",
            ["App language"] = "Язык приложения",
            ["English"] = "English",
            ["Russian"] = "Русский",
            ["On your first launch, WatchAtlas uses the browser language automatically. After that, your choice is saved here."] = "При первом запуске WatchAtlas автоматически использует язык браузера. После этого ваш выбор сохраняется здесь.",
            ["Backup"] = "Резервная копия",
            ["Export and restore your library"] = "Экспорт и восстановление библиотеки",
            ["Download a backup file with your library and settings, or restore everything from a saved file."] = "Скачайте резервную копию библиотеки и настроек или восстановите всё из сохранённого файла.",
            ["Export backup"] = "Экспорт резервной копии",
            ["Exports the full library snapshot, including movies, series, seasons, episodes, and current app settings."] = "Экспортирует полный снимок библиотеки, включая фильмы, сериалы, сезоны, эпизоды и текущие настройки приложения.",
            ["Preparing Export..."] = "Подготовка экспорта...",
            ["Export JSON Backup"] = "Экспортировать JSON-резервную копию",
            ["Import backup"] = "Импорт резервной копии",
            ["The current MVP replaces local data after confirmation, so import is safe from partial merges but will overwrite the current browser snapshot."] = "Текущая версия MVP заменяет локальные данные после подтверждения, поэтому импорт защищён от частичных слияний, но перезапишет текущий снимок браузера.",
            ["Choose Backup File"] = "Выбрать файл резервной копии",
            ["Tip:"] = "Совет:",
            ["export a backup before clearing data or moving to another device."] = "экспортируйте резервную копию перед очисткой данных или переносом на другое устройство.",
            ["Import:"] = "Импорт:",
            ["only valid WatchAtlas backup files are accepted."] = "принимаются только корректные резервные копии WatchAtlas.",
            ["Restore:"] = "Восстановление:",
            ["importing replaces the current library and preferences with the contents of the selected backup."] = "импорт заменяет текущую библиотеку и настройки содержимым выбранной резервной копии.",
            ["Danger Zone"] = "Опасная зона",
            ["Reset local WatchAtlas data"] = "Сбросить локальные данные WatchAtlas",
            ["This permanently clears your library and preferences on this device."] = "Это навсегда очистит вашу библиотеку и настройки на этом устройстве.",
            ["Reset library and preferences"] = "Сбросить библиотеку и настройки",
            ["Use this if you want to remove everything and start over."] = "Используйте это, если хотите удалить всё и начать заново.",
            ["Reset All Local Data"] = "Сбросить все локальные данные",
            ["Replace local data with this backup?"] = "Заменить локальные данные этой резервной копией?",
            ["Import replaces the current browser snapshot. Export first if you want an extra recovery point."] = "Импорт заменяет текущий снимок браузера. Сначала экспортируйте данные, если хотите дополнительную точку восстановления.",
            ["the selected file"] = "выбранный файл",
            ["The current library, filters, theme preferences, and saved settings will be replaced immediately after confirmation."] = "Текущая библиотека, фильтры, настройки темы и сохранённые параметры будут заменены сразу после подтверждения.",
            ["Cancel"] = "Отмена",
            ["Replace Local Data"] = "Заменить локальные данные",
            ["Reset all local data?"] = "Сбросить все локальные данные?",
            ["This clears your saved library and preferences permanently."] = "Это навсегда очистит сохранённую библиотеку и настройки.",
            ["All movies, series, seasons, episodes, cover images, filters, and preferences will be removed from this device."] = "Все фильмы, сериалы, сезоны, эпизоды, обложки, фильтры и настройки будут удалены с этого устройства.",
            ["Reset Everything"] = "Сбросить всё",
            ["Backup exported."] = "Резервная копия экспортирована.",
            ["A full JSON snapshot of your library and settings has been downloaded."] = "Полный JSON-снимок вашей библиотеки и настроек был скачан.",
            ["Backup export failed."] = "Не удалось экспортировать резервную копию.",
            ["Import failed."] = "Импорт не удался.",
            ["The selected backup could not be imported."] = "Не удалось импортировать выбранную резервную копию.",
            ["Backup restored."] = "Резервная копия восстановлена.",
            ["The selected backup replaced the current local data."] = "Выбранная резервная копия заменила текущие локальные данные.",
            ["Backup restore failed."] = "Не удалось восстановить резервную копию.",
            ["Local data reset."] = "Локальные данные сброшены.",
            ["The library and saved preferences were cleared from browser storage."] = "Библиотека и сохранённые настройки были удалены из хранилища браузера.",
            ["Reset failed."] = "Сброс не удался.",
            ["Dismiss notification"] = "Закрыть уведомление",
            ["Sort"] = "Сортировка",
            ["Genre"] = "Жанр",
            ["All genres"] = "Все жанры",
            ["Selected Series"] = "Выбранный сериал",
            ["All series"] = "Все сериалы",
            ["Everything is visible."] = "Показаны все элементы.",
            ["Quick Filters"] = "Быстрые фильтры",
            ["All Items"] = "Все элементы",
            ["Movies"] = "Фильмы",
            ["Series"] = "Сериал",
            ["Completed"] = "Завершено",
            ["In Progress"] = "В процессе",
            ["Not Started"] = "Не начато",
            ["Descending"] = "По убыванию",
            ["Ascending"] = "По возрастанию",
            ["Reset Filters"] = "Сбросить фильтры",
            ["Your library is still empty."] = "Ваша библиотека пока пуста.",
            ["Start by adding your first movie or series. Once it is saved, this page becomes your main browsing hub for search, filters, and progress tracking."] = "Начните с добавления первого фильма или сериала. После сохранения эта страница станет вашим основным местом для поиска, фильтров и отслеживания прогресса.",
            ["Add First Entry"] = "Добавить первую запись",
            ["Open Settings"] = "Открыть настройки",
            ["No Results"] = "Ничего не найдено",
            ["Nothing matches these filters."] = "Ничего не соответствует этим фильтрам.",
            ["Try clearing one or two filters, broadening the search text, or switching the media type and status filters back to a wider view."] = "Попробуйте снять один или два фильтра, расширить текст поиска или вернуть более широкий тип медиа и статус.",
            ["Add New Item"] = "Добавить новый элемент",
            ["Watch Time"] = "Время просмотра",
            ["Runtime"] = "Хронометраж",
            ["Movie"] = "Фильм",
            ["Watched"] = "Просмотрено",
            ["Unwatched"] = "Не просмотрено",
            ["Progress"] = "Прогресс",
            ["Tracked Time"] = "Отслеживаемое время",
            ["Created"] = "Создано",
            ["Updated"] = "Обновлено",
            ["No description yet. Open details to add notes, cover art, and richer tracking info."] = "Описания пока нет. Откройте детали, чтобы добавить заметки, обложку и больше информации.",
            ["No description yet. Open details to enrich this entry."] = "Описания пока нет. Откройте детали, чтобы дополнить эту запись.",
            ["Draft"] = "Черновик",
            ["Not tracked yet"] = "Пока не отслеживается",
            ["Completion Overview"] = "Обзор завершённости",
            ["A quick breakdown of watched versus remaining items across movies, series, and episodes."] = "Быстрая разбивка просмотренного и оставшегося по фильмам, сериалам и эпизодам.",
            ["Movie Watch Rate"] = "Доля просмотренных фильмов",
            ["Episode Watch Rate"] = "Доля просмотренных эпизодов",
            ["Completed Series"] = "Завершённые сериалы",
            ["Watch Time Summary"] = "Сводка времени просмотра",
            ["Compare watched movie runtime, watched episode runtime, and how much tracked time is still left unseen."] = "Сравните время просмотренных фильмов, эпизодов и объём отслеживаемого времени, который ещё не просмотрен.",
            ["Watched movie runtime."] = "Время просмотренных фильмов.",
            ["Watched episode runtime."] = "Время просмотренных эпизодов.",
            ["Total Watched"] = "Всего просмотрено",
            ["Combined watched runtime."] = "Суммарное время просмотра.",
            ["Remaining"] = "Осталось",
            ["Tracked runtime not watched yet."] = "Отслеживаемое время, которое ещё не просмотрено.",
            ["Pending"] = "Ожидает",
            ["A clear overview of your watch progress."] = "Наглядный обзор вашего прогресса просмотра.",
            ["Crunching totals, watch-time summaries, and completion breakdowns from browser storage."] = "Подсчитываем общие показатели, сводки времени просмотра и разбивку завершённости из хранилища браузера.",
            ["Loading your watch progress."] = "Загружаем ваш прогресс просмотра.",
            ["Movies Watched"] = "Просмотренные фильмы",
            ["Series Added"] = "Добавленные сериалы",
            ["Episodes Watched"] = "Просмотренные эпизоды",
            ["Total Watch Time"] = "Общее время просмотра",
            ["Sum of watched movie and episode runtime."] = "Сумма времени просмотренных фильмов и эпизодов.",
            ["Rankings"] = "Рейтинги",
            ["Top series by watched duration."] = "Топ сериалов по времени просмотра.",
            ["Series ordered by the amount of watched runtime already logged in the library."] = "Сериалы, отсортированные по уже зафиксированному времени просмотра.",
            ["No tracked series runtime yet. Add a series with seasons and watched episodes to see rankings here."] = "Пока нет отслеживаемого времени сериалов. Добавьте сериал с сезонами и просмотренными эпизодами, чтобы увидеть рейтинг.",
            ["Most episodes watched."] = "Больше всего просмотренных эпизодов.",
            ["Series ranked by watched episode count, then by logged watch time."] = "Сериалы, отсортированные по числу просмотренных эпизодов, затем по времени просмотра.",
            ["No watched episodes yet. Start tracking episode progress to populate this ranking."] = "Пока нет просмотренных эпизодов. Начните отмечать прогресс, чтобы заполнить этот рейтинг.",
            ["Currently watching."] = "Смотрите сейчас.",
            ["Series that are in progress right now, ranked by watched episodes and active watch time."] = "Сериалы в процессе просмотра, отсортированные по просмотренным эпизодам и активному времени.",
            ["No in-progress series yet. Once a series is partly watched, it will show up here."] = "Пока нет сериалов в процессе. Когда сериал будет частично просмотрен, он появится здесь.",
            ["Genres"] = "Жанры",
            ["Watch time by genre."] = "Время просмотра по жанрам.",
            ["Watched movie runtime and watched episode runtime grouped by library genres."] = "Время просмотренных фильмов и эпизодов, сгруппированное по жанрам библиотеки.",
            ["No watched runtime has been matched to genres yet. Add genres and watched entries to see the breakdown."] = "Пока просмотренное время не связано с жанрами. Добавьте жанры и просмотренные записи, чтобы увидеть разбивку.",
            ["Seasons"] = "Сезоны",
            ["Top seasons by watched runtime."] = "Топ сезонов по времени просмотра.",
            ["Season totals are calculated from episode data and ranked across the whole library."] = "Итоги по сезонам рассчитываются по данным эпизодов и ранжируются по всей библиотеке.",
            ["No season runtime has been logged yet. Mark episodes as watched to populate these cards."] = "Пока нет данных по времени сезонов. Отмечайте эпизоды просмотренными, чтобы заполнить эти карточки.",
            ["Loading your watch library."] = "Загружаем вашу библиотеку просмотра.",
            ["Getting your movies, series, and watch progress ready."] = "Подготавливаем фильмы, сериалы и прогресс просмотра.",
            ["Your personal watch library starts here."] = "Ваша личная библиотека просмотра начинается здесь.",
            ["Track what you've watched, see your progress, and jump back into your library anytime."] = "Отмечайте просмотренное, следите за прогрессом и возвращайтесь в библиотеку в любой момент.",
            ["Welcome"] = "Добро пожаловать",
            ["A soft, simple place to keep up with what you've watched."] = "Мягкое и простое место, чтобы помнить, что вы уже посмотрели.",
            ["Add films and series, follow episode progress, and keep an eye on your total watch time without losing the calm feel of a personal collection."] = "Добавляйте фильмы и сериалы, следите за прогрессом по эпизодам и общим временем просмотра без ощущения перегруженности.",
            ["Movies you've already marked as watched."] = "Фильмы, которые вы уже отметили как просмотренные.",
            ["Series currently in your library."] = "Сериалы, которые сейчас есть в вашей библиотеке.",
            ["Episodes you've checked off so far."] = "Эпизоды, которые вы уже отметили.",
            ["Time you've spent watching across the library."] = "Время, которое вы потратили на просмотр по всей библиотеке.",
            ["Recently updated"] = "Недавно обновлённые",
            ["Least recently updated"] = "Давно не обновлялись",
            ["Recently added"] = "Недавно добавленные",
            ["Oldest first"] = "Сначала старые",
            ["Title A-Z"] = "Название А-Я",
            ["Title Z-A"] = "Название Я-А",
            ["Highest rating"] = "Самый высокий рейтинг",
            ["Lowest rating"] = "Самый низкий рейтинг",
            ["Most progress"] = "Наибольший прогресс",
            ["Least progress"] = "Наименьший прогресс",
            ["Browse your full watch collection."] = "Просматривайте всю свою коллекцию просмотра.",
            ["Search, filter, sort, and switch layouts across your full movie and series collection. The browsing view updates instantly from shared browser-backed state."] = "Ищите, фильтруйте, сортируйте и переключайте режимы просмотра для всей коллекции фильмов и сериалов. Вид обновляется мгновенно благодаря общему состоянию в браузере.",
            ["A few details need a quick check."] = "Некоторые детали стоит быстро проверить.",
            ["Accepted: absolute http/https image URLs."] = "Поддерживаются абсолютные http/https URL изображений.",
            ["Accepted: JPG, PNG, WEBP, GIF. Max size: {0} MB."] = "Поддерживаются JPG, PNG, WEBP, GIF. Максимальный размер: {0} МБ.",
            ["Add a cover image from the edit page to give the series page a stronger visual identity."] = "Добавьте обложку на странице редактирования, чтобы страница сериала выглядела выразительнее.",
            ["Add a cover image from the edit page to make this movie card feel more complete."] = "Добавьте обложку на странице редактирования, чтобы карточка фильма выглядела завершённее.",
            ["Add a movie to your library."] = "Добавьте фильм в свою библиотеку.",
            ["Add a series to your library."] = "Добавьте сериал в свою библиотеку.",
            ["Add Episode"] = "Добавить эпизод",
            ["Add Season"] = "Добавить сезон",
            ["Add Seasons"] = "Добавить сезоны",
            ["Add Seasons From Markdown"] = "Добавить сезоны из Markdown",
            ["Add the title, cover, and any details you want to remember."] = "Добавьте название, обложку и любые детали, которые хотите сохранить.",
            ["Apply To All Episodes"] = "Применить ко всем эпизодам",
            ["Back to Details"] = "Назад к деталям",
            ["Build seasons and episodes with quick generation, watch-state bulk actions, and reusable runtime tools."] = "Создавайте сезоны и эпизоды с быстрой генерацией, массовыми действиями по статусу просмотра и инструментами для хронометража.",
            ["Bulk Actions"] = "Массовые действия",
            ["Choose an external cover URL or upload a local series key art image."] = "Укажите внешний URL обложки или загрузите локальное изображение сериала.",
            ["Choose an external cover URL or upload a poster image from this device."] = "Укажите внешний URL постера или загрузите изображение с этого устройства.",
            ["Completion"] = "Завершённость",
            ["Copy Prompt"] = "Скопировать промпт",
            ["Copy this prepared prompt and ask your assistant for import-ready markdown."] = "Скопируйте этот подготовленный промпт и попросите ассистента сгенерировать markdown для импорта.",
            ["Cover preview"] = "Предпросмотр обложки",
            ["Create a movie first or return to the library to pick an existing item."] = "Сначала создайте фильм или вернитесь в библиотеку, чтобы выбрать существующий элемент.",
            ["Create a movie first, then return here to review metadata, watch status, and notes."] = "Сначала создайте фильм, затем вернитесь сюда, чтобы просмотреть метаданные, статус просмотра и заметки.",
            ["Create a series first or return to the library to pick an existing entry."] = "Сначала создайте сериал или вернитесь в библиотеку, чтобы выбрать существующую запись.",
            ["Create a series first, then return here to review seasons, episodes, and progress."] = "Сначала создайте сериал, затем вернитесь сюда, чтобы просмотреть сезоны, эпизоды и прогресс.",
            ["Create Movie"] = "Создать фильм",
            ["Create Series"] = "Создать сериал",
            ["Delete Movie"] = "Удалить фильм",
            ["Delete Series"] = "Удалить сериал",
            ["Delete this movie?"] = "Удалить этот фильм?",
            ["Delete this series?"] = "Удалить этот сериал?",
            ["Description"] = "Описание",
            ["Duration"] = "Длительность",
            ["Duration (minutes)"] = "Длительность (минуты)",
            ["Edit Movie"] = "Редактировать фильм",
            ["Edit Series"] = "Редактировать сериал",
            ["Enter a valid absolute http/https image URL."] = "Введите корректный абсолютный http/https URL изображения.",
            ["Episode #"] = "Эпизод №",
            ["Episodes"] = "Эпизоды",
            ["External Image URL"] = "Внешний URL изображения",
            ["Fill From Markdown"] = "Заполнить из Markdown",
            ["Fill Series From Markdown"] = "Заполнить сериал из Markdown",
            ["Fill this movie from markdown"] = "Заполнить фильм из markdown",
            ["Fill this series from markdown"] = "Заполнить сериал из markdown",
            ["For example: `Season 3` or `Season 3 and Season 4`."] = "Например: `Season 3` или `Season 3 and Season 4`.",
            ["Generate clean markdown first, then paste it here."] = "Сначала сгенерируйте чистый markdown, затем вставьте его сюда.",
            ["Generate Count"] = "Количество для генерации",
            ["Generate Episodes"] = "Сгенерировать эпизоды",
            ["Getting this movie ready to edit."] = "Подготавливаем фильм к редактированию.",
            ["Getting this series ready to edit."] = "Подготавливаем сериал к редактированию.",
            ["How to use it"] = "Как использовать",
            ["Image uploaded and stored locally for this item."] = "Изображение загружено и сохранено локально для этого элемента.",
            ["Imported with a few notes."] = "Импорт выполнен с замечаниями.",
            ["It may have been removed, or the link may be out of date."] = "Возможно, запись была удалена или ссылка устарела.",
            ["Last Updated"] = "Последнее обновление",
            ["Library search and quick actions"] = "Поиск по библиотеке и быстрые действия",
            ["Loading movie details."] = "Загружаем детали фильма.",
            ["Loading movie editor."] = "Загружаем редактор фильма.",
            ["Loading series details."] = "Загружаем детали сериала.",
            ["Loading series editor."] = "Загружаем редактор сериала.",
            ["Mark Entire Series Watched"] = "Отметить весь сериал как просмотренный",
            ["Mark Unwatched"] = "Снять отметку просмотра",
            ["Mark Watched"] = "Отметить просмотренным",
            ["Marked as watched"] = "Отмечено как просмотренное",
            ["Marked as watched."] = "Отмечено как просмотренное.",
            ["Media Type"] = "Тип медиа",
            ["Movie changes could not be saved."] = "Не удалось сохранить изменения фильма.",
            ["Movie could not be created."] = "Не удалось создать фильм.",
            ["Movie could not be deleted."] = "Не удалось удалить фильм.",
            ["Movie cover preview"] = "Предпросмотр обложки фильма",
            ["Movie created."] = "Фильм создан.",
            ["Movie deleted."] = "Фильм удалён.",
            ["Movie Details"] = "Детали фильма",
            ["Movie details added."] = "Детали фильма добавлены.",
            ["Movie Editor"] = "Редактор фильма",
            ["Movie not found."] = "Фильм не найден.",
            ["Movie saved."] = "Фильм сохранён.",
            ["Movie title for AI"] = "Название фильма для ИИ",
            ["New seasons and episodes were appended to the current series draft."] = "Новые сезоны и эпизоды добавлены в текущий черновик сериала.",
            ["No cover selected yet"] = "Обложка пока не выбрана",
            ["No episodes in this season yet."] = "В этом сезоне пока нет эпизодов.",
            ["No movie found."] = "Фильм не найден.",
            ["No notes have been added for this movie yet."] = "Для этого фильма пока не добавлено заметок.",
            ["No seasons have been added yet. Open the editor to create the first season and generate episodes."] = "Сезоны ещё не добавлены. Откройте редактор, чтобы создать первый сезон и сгенерировать эпизоды.",
            ["No seasons yet. Add one to start structuring the series."] = "Сезонов пока нет. Добавьте один, чтобы начать структурировать сериал.",
            ["No series found."] = "Сериал не найден.",
            ["No watch date"] = "Дата просмотра отсутствует",
            ["Not set"] = "Не задано",
            ["Notes"] = "Заметки",
            ["Open each season to inspect episode progress, runtime, and watched dates."] = "Откройте любой сезон, чтобы посмотреть прогресс эпизодов, длительность и даты просмотра.",
            ["Open Library"] = "Открыть библиотеку",
            ["Paste it into your assistant and generate the markdown from the prepared prompt."] = "Вставьте это в своего ассистента и сгенерируйте markdown по подготовленному промпту.",
            ["Paste markdown first."] = "Сначала вставьте markdown.",
            ["Paste one clean markdown document for a single movie, or copy the AI prompt below to generate it automatically in the right format."] = "Вставьте один аккуратный markdown-документ для фильма или скопируйте AI-промпт ниже, чтобы сгенерировать его автоматически в правильном формате.",
            ["Paste one clean markdown document for a single series, or copy the AI prompt below to generate it automatically in the right format."] = "Вставьте один аккуратный markdown-документ для сериала или скопируйте AI-промпт ниже, чтобы сгенерировать его автоматически в правильном формате.",
            ["Paste one or more season blocks, or copy the AI prompt below to generate only the seasons you still need to add."] = "Вставьте один или несколько блоков сезонов или скопируйте AI-промпт ниже, чтобы сгенерировать только те сезоны, которые ещё нужно добавить.",
            ["Please review the form fields."] = "Проверьте поля формы.",
            ["Please review the form."] = "Проверьте форму.",
            ["Please review the series form."] = "Проверьте форму сериала.",
            ["Progress, watched runtime, and per-season completion are calculated automatically from episode state."] = "Прогресс, время просмотра и завершённость по сезонам автоматически рассчитываются на основе состояния эпизодов.",
            ["Prompt copied."] = "Промпт скопирован.",
            ["Prompt for ChatGPT or Claude"] = "Промпт для ChatGPT или Claude",
            ["Prompt for missing seasons"] = "Промпт для недостающих сезонов",
            ["Quick Edit"] = "Быстрое редактирование",
            ["Rating"] = "Оценка",
            ["Reading the selected movie from browser storage."] = "Читаем выбранный фильм из хранилища браузера.",
            ["Reading the selected series from browser storage."] = "Читаем выбранный сериал из хранилища браузера.",
            ["Remove"] = "Удалить",
            ["Remove Image"] = "Удалить изображение",
            ["Return to Library"] = "Вернуться в библиотеку",
            ["Runtime & progress"] = "Хронометраж и прогресс",
            ["Save Changes"] = "Сохранить изменения",
            ["Season Breakdown"] = "Разбивка по сезонам",
            ["Season Completion"] = "Завершённость сезона",
            ["Season Editor"] = "Редактор сезонов",
            ["Season Number"] = "Номер сезона",
            ["Season Progress"] = "Прогресс сезона",
            ["Season Title"] = "Название сезона",
            ["Seasons added."] = "Сезоны добавлены.",
            ["See a full series example"] = "Посмотреть полный пример сериала",
            ["See a movie example"] = "Посмотреть пример фильма",
            ["See a season block example"] = "Посмотреть пример блока сезона",
            ["Series changes could not be saved."] = "Не удалось сохранить изменения сериала.",
            ["Series Completion"] = "Завершённость сериала",
            ["Series could not be created."] = "Не удалось создать сериал.",
            ["Series could not be deleted."] = "Не удалось удалить сериал.",
            ["Series cover preview"] = "Предпросмотр обложки сериала",
            ["Series created."] = "Сериал создан.",
            ["Series deleted."] = "Сериал удалён.",
            ["Series Details"] = "Детали сериала",
            ["Series details added."] = "Детали сериала добавлены.",
            ["Series Editor"] = "Редактор сериала",
            ["Series not found."] = "Сериал не найден.",
            ["Series saved."] = "Сериал сохранён.",
            ["Series title for AI"] = "Название сериала для ИИ",
            ["Set up the series, then add seasons and episodes whenever you're ready."] = "Настройте сериал, а затем добавляйте сезоны и эпизоды, когда будете готовы.",
            ["Shared Duration"] = "Общая длительность",
            ["Some movie details still need attention before save."] = "Некоторые детали фильма требуют внимания перед сохранением.",
            ["Some series details still need attention before save."] = "Некоторые детали сериала требуют внимания перед сохранением.",
            ["Store optional watch date information for this movie."] = "Сохранять необязательную дату просмотра для этого фильма.",
            ["Summary"] = "Сводка",
            ["The details route is ready, but the requested movie was not found in browser storage."] = "Маршрут деталей готов, но запрошенный фильм не найден в хранилище браузера.",
            ["The details route is ready, but the requested series was not found in browser storage."] = "Маршрут деталей готов, но запрошенный сериал не найден в хранилище браузера.",
            ["The draft was filled from your markdown."] = "Черновик был заполнен из вашего markdown.",
            ["The full prompt will be copied to your clipboard."] = "Полный промпт будет скопирован в буфер обмена.",
            ["The image URL could not be loaded for preview."] = "URL изображения не удалось загрузить для предпросмотра.",
            ["The markdown could not be applied."] = "Не удалось применить markdown.",
            ["The movie form was filled from your markdown."] = "Форма фильма была заполнена из вашего markdown.",
            ["The new movie has been saved in your library."] = "Новый фильм сохранён в вашей библиотеке.",
            ["The new series has been saved in your library."] = "Новый сериал сохранён в вашей библиотеке.",
            ["The series form was filled from your markdown."] = "Форма сериала была заполнена из вашего markdown.",
            ["The statistics service now centralizes totals, watch-time summaries, completion breakdowns, and ranked series or season views so analytics stay consistent everywhere."] = "Сервис статистики теперь централизует итоги, сводки по времени просмотра, разбивку завершённости и рейтинги сериалов и сезонов, чтобы аналитика оставалась согласованной везде.",
            ["This movie could not be found."] = "Этот фильм не удалось найти.",
            ["This movie is in your library and ready for watch progress, notes, and metadata updates."] = "Этот фильм находится в вашей библиотеке и готов для отметок прогресса, заметок и обновления метаданных.",
            ["This movie is not in the library yet."] = "Этого фильма пока нет в библиотеке.",
            ["This permanently removes the movie and its stored watch details from browser storage."] = "Это навсегда удалит фильм и сохранённые данные о просмотре из хранилища браузера.",
            ["This permanently removes the series and all stored seasons and episodes from browser storage."] = "Это навсегда удалит сериал и все сохранённые сезоны и эпизоды из хранилища браузера.",
            ["This season does not contain any episodes yet."] = "Этот сезон пока не содержит эпизодов.",
            ["This series could not be found."] = "Этот сериал не удалось найти.",
            ["This series is not in the library yet."] = "Этого сериала пока нет в библиотеке.",
            ["This series is saved and ready for seasons, episodes, and watch tracking."] = "Этот сериал сохранён и готов для добавления сезонов, эпизодов и отслеживания просмотра.",
            ["This title will be inserted into the copied prompt automatically."] = "Это название будет автоматически подставлено в скопированный промпт.",
            ["Title"] = "Название",
            ["Total episodes tracked."] = "Всего отслеживаемых эпизодов.",
            ["Tracked in this series."] = "Отслеживается в этом сериале.",
            ["Update the details, notes, and watch status for this movie."] = "Обновите детали, заметки и статус просмотра этого фильма.",
            ["Update the series details, seasons, and episodes."] = "Обновите детали сериала, сезоны и эпизоды.",
            ["Upload"] = "Загрузить",
            ["Upload Image File"] = "Загрузить файл изображения",
            ["URL"] = "URL",
            ["Use an image link or upload a file from your device."] = "Используйте ссылку на изображение или загрузите файл со своего устройства.",
            ["Use this when you already have a series draft and want your assistant to generate only the missing seasons."] = "Используйте это, если у вас уже есть черновик сериала и вы хотите, чтобы ассистент сгенерировал только недостающие сезоны.",
            ["Watch Status"] = "Статус просмотра",
            ["Watched Date"] = "Дата просмотра",
            ["Watched episode time."] = "Время просмотренных эпизодов.",
            ["Watched Episodes"] = "Просмотренные эпизоды",
            ["Watched movie details, runtime, and notes are stored locally and ready to edit."] = "Данные о просмотре фильма, хронометраж и заметки сохранены локально и готовы к редактированию.",
            ["Watched Movies"] = "Просмотренные фильмы",
            ["Which seasons to add"] = "Какие сезоны добавить",
            ["Your movie details were updated successfully."] = "Детали фильма успешно обновлены.",
            ["Your series details were updated successfully."] = "Детали сериала успешно обновлены.",
            ["Add Media"] = "Добавить медиа",
            ["New Media"] = "Новое медиа",
            ["Add a movie or series to your library."] = "Добавьте фильм или сериал в свою библиотеку.",
            ["Fill in the details below and save it to your collection."] = "Заполните данные ниже и сохраните элемент в свою коллекцию.",
            ["Save Entry"] = "Сохранить запись",
            ["Media Details"] = "Детали медиа",
            ["This entry is not in the library yet."] = "Этой записи пока нет в библиотеке.",
            ["The route is ready for editing, but the requested item was not found in local storage."] = "Маршрут редактирования готов, но запрошенный элемент не найден в локальном хранилище.",
            ["Details"] = "Детали",
            ["No media entry found."] = "Медиаэлемент не найден.",
            ["Create a new item first, then return here to edit metadata, seasons, episodes, and watch progress."] = "Сначала создайте новый элемент, затем вернитесь сюда, чтобы редактировать метаданные, сезоны, эпизоды и прогресс просмотра.",
            ["Create New Item"] = "Создать новый элемент",
            ["Edit the stored item, adjust progress, and keep the browser-backed library in sync."] = "Редактируйте сохранённый элемент, меняйте прогресс и синхронизируйте библиотеку в браузере.",
            ["Delete Entry"] = "Удалить запись",
            ["Not available"] = "Недоступно",
            ["{0} season(s), {1} tracked episode(s), {2} watched."] = "{0} сезон(ов), {1} отслеживаемых эпизодов, {2} просмотрено.",
            ["{0}% complete with {1} of watched runtime."] = "{0}% завершено, просмотрено {1} времени.",
            ["This movie is marked as watched."] = "Этот фильм отмечен как просмотренный.",
            ["This movie is still marked as not watched."] = "Этот фильм всё ещё отмечен как непросмотренный.",
            ["Stored runtime: {0}."] = "Сохранённый хронометраж: {0}.",
            ["Not Found"] = "Не найдено",
            ["This page drifted out of orbit."] = "Эта страница сошла с орбиты.",
            ["The GitHub Pages fallback is configured, but this route still needs a matching page in the app."] = "Резервный маршрут GitHub Pages настроен, но этому пути всё ещё нужна соответствующая страница в приложении.",
            ["Routing"] = "Маршрутизация",
            ["Nothing is mapped here yet."] = "Здесь пока ничего не настроено.",
            ["Try returning to the dashboard or jump into the library shell while the rest of the watch-tracking features are built out."] = "Попробуйте вернуться на главную или открыть библиотеку, пока остальные функции трекинга ещё дорабатываются.",
            ["Go Home"] = "На главную",
            ["Rating: {0}"] = "Оценка: {0}",
            ["{0} cover preview"] = "Предпросмотр обложки: {0}",
            ["{0} of {1} match."] = "Совпадает: {0} из {1}.",
            ["{0} media item(s) and the saved app preferences from {1} are ready to restore."] = "{0} медиаэлементов и сохранённые настройки из файла {1} готовы к восстановлению.",
            ["{0} of {1} movies are marked watched."] = "{0} из {1} фильмов отмечены как просмотренные.",
            ["{0} completed, {1} in progress."] = "{0} завершено, {1} в процессе.",
            ["{0}/{1} tracked episodes watched."] = "{0} из {1} отслеживаемых эпизодов просмотрено.",
            ["{0} watched, {1} remaining."] = "{0} просмотрено, {1} осталось.",
            ["{0}/{1} episodes"] = "{0}/{1} эпизодов",
            ["Open details for {0}"] = "Открыть детали для {0}",
            ["Season {0}"] = "Сезон {0}",
            ["Episode {0}"] = "Эпизод {0}",
            ["{0} / {1} watched"] = "{0} / {1} просмотрено",
            ["The prompt could not be copied: {0}"] = "Не удалось скопировать промпт: {0}",
            ["\"{0}\" was removed from your library."] = "«{0}» был удалён из вашей библиотеки.",
            ["\"{0}\" and its tracked progress were removed."] = "«{0}» и весь его отслеживаемый прогресс были удалены."
        });

        AddTranslations(new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["Additional details"] = "Дополнительные детали",
            ["Add seasons from markdown"] = "Добавить сезоны из markdown",
            ["Cover Image"] = "Обложка",
            ["Copy this prompt into ChatGPT or another assistant, then paste the generated markdown back here."] = "Скопируйте этот промпт в ChatGPT или другой ассистент, затем вставьте сюда сгенерированный markdown.",
            ["Enter the exact movie title below and copy the AI prompt."] = "Введите ниже точное название фильма и скопируйте AI-промпт.",
            ["Enter the exact series title below and copy the AI prompt."] = "Введите ниже точное название сериала и скопируйте AI-промпт.",
            ["Enter the series title and the seasons you want to add, then copy the AI prompt."] = "Введите название сериала и сезоны, которые хотите добавить, затем скопируйте AI-промпт.",
            ["Fill Form"] = "Заполнить форму",
            ["Keep the answer as raw markdown with no code fences or extra text."] = "Сохраняйте ответ как чистый markdown без лишнего текста.",
            ["Paste prepared markdown here after generating it with your assistant."] = "Вставьте сюда готовый markdown после его генерации в вашем ассистенте.",
            ["Paste the returned markdown here as raw markdown with no code fences or extra text."] = "Вставьте сюда полученный markdown как чистый markdown без лишнего текста.",
            ["Paste the result here to append the new seasons to the current series draft."] = "Вставьте результат сюда, чтобы добавить новые сезоны в текущий черновик сериала.",
            ["Paste the returned markdown here without extra commentary and click Fill Form."] = "Вставьте сюда полученный markdown и нажмите «Заполнить форму».",
            ["Prompt for AI"] = "Промпт для AI",
            ["Season 3, Season 4"] = "Сезон 3, сезон 4"
        });
    }

    public static string Translate(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        return GetCurrentLanguage() == AppLanguage.Russian && Russian.TryGetValue(text, out var translation)
            ? translation
            : text;
    }

    public static string Format(string text, params object?[] args)
        => string.Format(CultureInfo.CurrentCulture, Translate(text), args);

    public static string FormatDuration(int minutes)
    {
        if (GetCurrentLanguage() == AppLanguage.Russian)
        {
            if (minutes <= 0)
            {
                return "0 мин";
            }

            var russianHours = minutes / 60;
            var russianRemainingMinutes = minutes % 60;

            return russianHours switch
            {
                0 => $"{russianRemainingMinutes} мин",
                _ when russianRemainingMinutes == 0 => $"{russianHours} ч",
                _ => $"{russianHours} ч {russianRemainingMinutes} мин"
            };
        }

        if (minutes <= 0)
        {
            return "0 min";
        }

        var hours = minutes / 60;
        var remainingMinutes = minutes % 60;

        return hours switch
        {
            0 => $"{remainingMinutes} min",
            _ when remainingMinutes == 0 => $"{hours}h",
            _ => $"{hours}h {remainingMinutes}m"
        };
    }

    public static string FormatCount(int count, string nounKey, string? plural = null)
    {
        if (GetCurrentLanguage() == AppLanguage.Russian)
        {
            var russianForms = nounKey switch
            {
                "movie" => ("фильм", "фильма", "фильмов"),
                "episode" => ("эпизод", "эпизода", "эпизодов"),
                "series" => ("сериал", "сериала", "сериалов"),
                "season" => ("сезон", "сезона", "сезонов"),
                "item" => ("элемент", "элемента", "элементов"),
                "title" => ("название", "названия", "названий"),
                _ => (nounKey, nounKey, nounKey)
            };

            return $"{count} {SelectRussianPlural(count, russianForms.Item1, russianForms.Item2, russianForms.Item3)}";
        }

        if (GetCurrentLanguage() != AppLanguage.Russian)
        {
            var pluralLabel = plural ?? $"{nounKey}s";
            return $"{count} {(count == 1 ? nounKey : pluralLabel)}";
        }

        return $"{count} {nounKey}";
    }

    public static AppLanguage GetCurrentLanguage()
        => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("ru", StringComparison.OrdinalIgnoreCase)
            ? AppLanguage.Russian
            : AppLanguage.English;

    private static string SelectRussianPlural(int count, string singular, string few, string many)
    {
        var normalized = Math.Abs(count) % 100;
        var lastDigit = normalized % 10;

        if (normalized is > 10 and < 20)
        {
            return many;
        }

        return lastDigit switch
        {
            1 => singular,
            2 or 3 or 4 => few,
            _ => many
        };
    }

    public static void AddTranslations(IEnumerable<KeyValuePair<string, string>> translations)
    {
        foreach (var (key, value) in translations)
        {
            Russian[key] = value;
        }
    }
}
