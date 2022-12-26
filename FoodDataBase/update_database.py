import sqlite3
from datetime import datetime
from datetime import date
import os
import pathlib
import xlrd
import sys

days_of_week = {1: "Понедельник",
                2: "Вторник",
                3: "Среда",
                4: "Четверг",
                5: "Пятница"}

to_match = {"Комплекс 5-11 классы (базовый)": "Завтрак",
            'Обед 5-11 классы': "Обед"}


def parse_food_from_excel(file_name):
    sheet = xlrd.open_workbook_xls(file_name).sheet_by_index(0)
    info = {"Завтрак": list(), "Обед": list()}

    for i in range(2, sheet.nrows):
        current_cell = sheet.cell_value(i, 0)
        if current_cell == 'Комплекс 5-11 классы (базовый)' or current_cell == 'Обед 5-11 классы':
            info[to_match[current_cell]].append(f"-{sheet.cell_value(i, 3)}\r\n".replace('*', ''))
            c = 1
            while sheet.cell_value(i + c, 0) == '':
                info[to_match[current_cell]].append(f"-{sheet.cell_value(i + c, 3)}\r\n".replace('*', ''))
                c += 1
                if sheet.cell_value(i + c, 0) != '' or c + i >= sheet.nrows:
                    break
    return info


def format_info(key, values):
    return f"*{key}*\r\n\r\n" + '\r\n'.join(values)


def update_database(db, parsed_food):
    c = db.cursor()

    command = 'INSERT INTO Menu VALUES (?, ?, ?)'
    values = (days_of_week[date.isoweekday(today)], format_info('Завтрак', parsed_food['Завтрак']),
              format_info('Обед', parsed_food['Обед']))
    c.execute(command, values)

    db.commit()


today = sys.argv[1]
file = f"{today}-sm.xls"
today = [int(e) for e in today.split("-")]
today = datetime(today[0], today[1], today[2])
dir_path = sys.argv[2]
path = pathlib.Path(dir_path, "excels", file)
db_path = dir_path.replace(r"FoodDataBase", r"SchoolBot\InfoSchoolBot\bin\Debug\net6.0")
if os.path.exists(str(path)):
    db = sqlite3.connect(db_path + r'\Base.db')
    parsed_excel = parse_food_from_excel(path)
    update_database(db, parsed_excel)
    db.close()
else:
    raise Exception(f'Excel for {today} is not found '
                    f'\n {path}')

