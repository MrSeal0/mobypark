sessions = {
    '123456789': {"id":"2","username":"gijsdegraaf","password":"1b1f4e666f54b55ccd2c701ec3435dba","name":"Gijs de Graaf","email":"gijsdegraaf@hotmail.com","phone":"+310698086312","role":"ADMIN","created_at":"2017-07-10","birth_year":1951,"active": True}
}

def add_session(token, user):
    sessions[token] = user

def remove_session(token):
    return sessions.pop(token, None)

def get_session(token):
    return sessions.get(token)
