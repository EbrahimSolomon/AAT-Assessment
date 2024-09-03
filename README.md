cd "C:\path\to\SQL Databases"



sqlite3 events.db



CREATE TABLE IF NOT EXISTS Events (
    EventId INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Date TEXT NOT NULL,
    TotalSeats INTEGER NOT NULL,
    AvailableSeats INTEGER NOT NULL
);



CREATE TABLE IF NOT EXISTS Registrations (
    RegistrationId INTEGER PRIMARY KEY AUTOINCREMENT,
    EventId INTEGER NOT NULL,
    UserEmail TEXT NOT NULL,
    RegistrationDate TEXT NOT NULL,
    ReferenceNumber TEXT NOT NULL,
    FOREIGN KEY (EventId) REFERENCES Events (EventId)
);


.tables



.schema Events
