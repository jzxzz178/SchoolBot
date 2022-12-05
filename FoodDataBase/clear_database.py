import sqlite3

days_of_week = {1: "Понедельник",
                2: "Вторник",
                3: "Среда",
                4: "Четверг",
                5: "Пятница"}


def clear_database(db):
    c = db.cursor()

    for key in days_of_week.keys():
        command = f'DELETE FROM "{days_of_week[key]}"'
        c.execute(command)

    db.commit()


data_base = sqlite3.connect('food_info.db')
clear_database(data_base)
data_base.close()
