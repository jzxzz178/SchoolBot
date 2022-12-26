import sqlite3
import sys
from datetime import datetime
from datetime import date

days_of_week = {1: "Понедельник",
                2: "Вторник",
                3: "Среда",
                4: "Четверг",
                5: "Пятница"}


def clear_database(db):
    c = db.cursor()

    command = f'DELETE FROM "{days_of_week[date.isoweekday(today)]}"'
    c.execute(command)

    db.commit()


today = [int(e) for e in sys.argv[1].split("-")]
today = datetime(today[0], today[1], today[2])
data_base = sqlite3.connect(sys.argv[2] + r'\food_info.db')
clear_database(data_base)
data_base.close()
