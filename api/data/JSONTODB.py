import json
import sqlite3


def load_json(filename):
    try:
        with open(filename, 'r') as file:
            return json.load(file)
    except FileNotFoundError:
        print(f"File {filename} not found.")
        return []

connection = sqlite3.connect('DataBase.db')
cursor  = connection.cursor()


def migrate_users():
    users = load_json('users.json')

    print("test")
    for user in users:
        print("current user:" + user['id'])
        active = 1 if user['active'] else 0

        cursor.execute('''
            INSERT INTO Users (ID, username, password, full_name, email, phone, role, creation_date, birth_year, active)
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        ''', (user['id'], user['username'], user['password'], user['name'], user['email'], user['phone'], user['role'], user['created_at'], user['birth_year'], active))
        connection.commit()
    
def migrate_vehicles():
    vehicles = load_json('vehicles.json')

    for vehicle in vehicles:
        print("current vehicle:" + vehicle['id'])
        cursor.execute('''
            INSERT INTO Vehicles (ID, user_id, license_plate, make, model, color, year, created_at)
            VALUES (?, ?, ?, ?, ?, ?, ?, ?)
        ''', (vehicle['id'], vehicle['user_id'], vehicle['license_plate'], vehicle['make'], vehicle['model'], vehicle['color'], vehicle['year'], vehicle['created_at']))
    connection.commit()
    
def migrate_parking_sessions():
    total_sessions = 0
    for i in range (1500):
        lotid = i + 1
        sessions = load_json(f"pdata/p{lotid}-sessions.json")
        for session in sessions.values():
            total_sessions += 1
            userid = cursor.execute('SELECT user_id FROM Vehicles WHERE license_plate = ?', (session['licenseplate'],)).fetchone()
            print(f"parkinglot: {lotid} | current session: {session['id']} | userid: {userid} |total sessions: {total_sessions}")
            #json items: id, parking_lot_id, licenseplate, started, stopped, user, duration_minutes, cost, payment_status
            #db items: ID, parking_lot_id, user_id, license_plate, start_time, end_time, duration_minutes, cost, payment_status
            cursor.execute('''
                INSERT INTO parking_sessions (ID, parking_lot_id, user_id, license_plate, start_time, end_time, duration_minutes, cost, payment_status)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)
            ''', (total_sessions, lotid, userid[0], session['licenseplate'], session['started'], session['stopped'], session['duration_minutes'], session['cost'], session['payment_status']))
        connection.commit()
        
migrate_parking_sessions()