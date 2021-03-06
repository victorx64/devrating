# Dev Rating: Система вознаграждения разработчика программного обеспечения

Виктор Семенов

## Введение

Ни фиксированная, ни почасовая оплаты программистов не учитывают точный объем и 
качество выполненных работ. Почасовая оплата, к тому же, мотивирует наоборот 
дольше выполнять работу. Попытки измерить производительность программиста по 
количеству закрытых задач приводят к снижению качества кода и повышению 
технического долга. Отсутствие метрик качества и объема работ программистов 
поощряет подхалимство и видимость работ, и не позволяет увидеть действительную 
производительность членов команды. Отсутствие единой системы слежения за 
качеством кода не позволяет программистам создавать репутацию, которая 
следовала бы за ними из проекта в проект.

Нам необходим способ определения награды программиста, основанный на качестве и 
размере написанного кода. Качество кода измерялось бы на основе статистики и 
было бы открытым. Награда бы не учитывала личные качества программиста и 
проведенную подготовительную работу, а только конечный результат.

## Качество кода

Качественный код — это код, который легко меняется. Чем меньше кода требуется 
удалить при изменении требований, тем качественнее написана кодовая база.

Таким образом, каждое удаление кода — это упущение написавшего этот код 
программиста. Задача программиста минимизировать возможные изменения, делая 
кодовую базу качественней. Модификация строки — это удаление старой, затем 
добавление новой строки.

Имея историю изменений кода, мы можем численно подсчитать качество кода 
программиста.

## Рейтинговая система

Введем рейтинговую систему, которая покажет чей код удаляется чаще, и кто чаще 
удаляет чужой код. Все программисты начинают с определенного рейтинга. Каждый 
раз, когда кто-то удаляет чужие строки мы будем повышать рейтинг удалившего и 
снижать рейтинг автора удаленной строки в соответствии с формулой Эло. Если 
программист за одну работу удалил строки нескольких авторов, сначала снижаем 
рейтинги авторов удаленных строк. Затем повышаем рейтинг программиста на сумму 
изменений рейтингов авторов удаленных строк.

Чтобы снизить риск резкого падения рейтинга из-за новых требований, программист 
мотивирован работать над несколькими функционалами или проектами параллельно.

## Награда

Награда пропорциональна количеству новых строк. Коэффициент пропорциональности 
зависит только от рейтинга исполнителя. Зависимость устанавливается проектным 
менеджером до начала работ.

Награда за работу не учитывает новый рейтинг, полученный за эту работу, чтобы 
у других членов команды была возможность снизить этот рейтинг до получения 
следующей награды. Таким образом, чем дольше выполняется работа, тем сильнее 
может упасть рейтинг.

Каждая работа должна пройти проверку кода, в которой определяется 
обоснованность изменений. Чтобы не допустить дублирование кода, нужно менять 
программистов, работающих в одном участке кода и ограничить максимальное число 
новых строк в одной работе.

Награда не учитывает сложность строки, тем мотивирует разбивать сложную 
логику на несколько строк. Чем равномернее сложность кода, тем легче он 
читается, и, соответственно, легче меняется.

Награда не учитывает сложность обнаружения дефекта. Если потребовались 
существенные усилия, чтобы обнаружить дефектный участок, значит код требует 
улучшений. Программист может либо сам улучшить код и получить за это награду, 
либо создать задачу с указанием сложного участка.

## Вывод

Мы получили способ точного расчета награды за изменение кода. Данный метод 
мотивирует программистов разбираться в бизнес-проблемах заказчика, постоянно 
улучшать код, писать тесты и не привязываться к одному функционалу. Смена 
программистов обезопасит проект от текучки и повысит поддерживаемость. 
Рейтинг позволяет не терять репутацию программиста при переходе на другой 
проект или компанию.
