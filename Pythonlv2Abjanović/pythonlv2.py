from tkinter import *
import tkinter.filedialog

import threading as thr
import socket


class GrafLik():
    boja = 0
    tocka = 0

    def __init__(self, boja, tocka):
        self.boja = boja
        self.tocka = tocka

    def SetColor(self, boja):
        '''Metoda koja postavlja boju'''
        self.boja = boja

    def GetColor(self):
        '''Metoda koja vraca boju'''
        return self.boja

    def Draw(self):
        pass


class Linija(GrafLik):
    tocka2 = 0

    def __init__(self, boja, tocka, tocka2):
        GrafLik.__init__(self, boja, tocka)
        self.tocka2 = tocka2

    def Draw(self, Canvas):
        Canvas.create_line(
            self.tocka[0], self.tocka[1], self.tocka2[0], self.tocka2[1], fill=self.boja)


class Trokut(Linija):
    tocka3 = 0

    def __init__(self, boja, tocka, tocka2, tocka3):
        Linija.__init__(self, boja, tocka, tocka2)
        self.tocka3 = tocka3

    def Draw(self, Canvas):
        Canvas.create_polygon(self.tocka[0], self.tocka[1], self.tocka2[0],
                              self.tocka2[1], self.tocka3[0], self.tocka3[1], outline=self.boja, fill='')


class Pravokutnik(GrafLik):
    visina = 0
    sirina = 0

    def __init__(self, boja, tocka, visina, sirina):
        GrafLik.__init__(self, boja, tocka)
        self.visina = visina
        self.sirina = sirina

    def Draw(self, Canvas):
        Canvas.create_rectangle(self.tocka[0], self.tocka[1], float(
            self.tocka[0])+self.sirina, float(self.tocka[1])+self.visina, outline=self.boja, fill='')


class Poligon(GrafLik):
    tocke = 0

    def __init__(self, boja, tocka, tocke):
        GrafLik.__init__(self, boja, tocka)
        self.tocke = tocke
        self.tocke += tocka

    def Draw(self, Canvas):
        Canvas.create_polygon(self.tocke, outline=self.boja, fill="")


class Kruznica(GrafLik):
    polumjer = 0

    def __init__(self, boja, tocka, polumjer):
        GrafLik.__init__(self, boja, tocka)
        self.polumjer = polumjer

    def Draw(self, Canvas):
        Canvas.create_oval(float(self.tocka[0])-float(self.polumjer), float(self.tocka[1])-float(self.polumjer), float(
            self.tocka[0]) + float(self.polumjer), float(self.tocka[1]) + float(self.polumjer), outline=self.boja, fill='')


class Elipsa(Kruznica):
    Ypolumjer = 0

    def __init__(self, boja, tocka, polumjer, Ypolumjer):
        Kruznica.__init__(self, boja, tocka, polumjer)
        self.Ypolumjer = Ypolumjer

    def Draw(self, Canvas):
        Canvas.create_oval(float(self.tocka[0])-float(self.polumjer), float(self.tocka[1])-float(self.Ypolumjer), float(
            self.tocka[0])+float(self.polumjer), float(self.tocka[1])+float(self.Ypolumjer), outline=self.boja, fill='')


class Application(Frame):

    def CreateWidgets(self):
        self.C = Canvas(self, bg='#999999', height=600, width=800)
        self.C.pack()

        self.m = Menu(self.master)
        self.filemenu = Menu(self.m, tearoff=0)
        self.filemenu.add_command(label="Open", command=self.FunkcijaOpen)
        self.filemenu.add_separator()
        self.filemenu.add_command(label="Exit", command=self.quit)
        self.m.add_cascade(label="File", menu=self.filemenu)

        self.editmenu = Menu(self.m, tearoff=0)
        self.editmenu.add_command(label="Start Server", command=self.FunkcijaStartServer)
        self.m.add_cascade(label="Server", menu=self.editmenu)

        self.master.config(menu=self.m)


    def FunkcijaStartServer(self):
        ServerThr = thr.Thread(target=StServer, args=(self.C,))
        ServerThr.daemon = True
        ServerThr.start()
        

    def FunkcijaOpen(self):
        Ofile = tkinter.filedialog.askopenfile()
        text = Ofile.readlines()
        polje_prvih_rijeci = []
        polje_boja = []
        polje_brojeva = []
        for line in text:
            prva_rijec = line.split(' ')[0]
            druga_rijec = line.split(' ')[1]
            kordinate = line.split(' ')[2:]
            kordinate[-1] = kordinate[-1].strip()  # uklanja '/n'
            if kordinate[-1] == '':
                kordinate.pop()  # uklanja zadnji element linije ako je ''

            polje_brojeva.append(kordinate)
            polje_prvih_rijeci.append(prva_rijec)
            polje_boja.append(druga_rijec)

        print(polje_prvih_rijeci)
        print(polje_boja)
        print(polje_brojeva)
        print(len(polje_prvih_rijeci))
        print(len(polje_boja))
        print(len(polje_brojeva))

        a = 0
        while a < len(polje_prvih_rijeci):
            if polje_prvih_rijeci[a] == 'Polygon':
                Obj = Poligon(
                    polje_boja[a], polje_brojeva[a][0:2], polje_brojeva[a][2:])
                Obj.Draw(self.C)

            elif polje_prvih_rijeci[a] == 'Line':
                Obj = Linija(polje_boja[a], polje_brojeva[a]
                             [0:2], polje_brojeva[a][2:])
                Obj.Draw(self.C)

            elif polje_prvih_rijeci[a] == 'Triangle':
                Obj = Trokut(polje_boja[a], polje_brojeva[a][0:2],
                             polje_brojeva[a][2:4], polje_brojeva[a][4:])
                Obj.Draw(self.C)

            elif polje_prvih_rijeci[a] == 'Rectangle':

                Obj = Pravokutnik(polje_boja[a], polje_brojeva[a][0:2], float(
                    polje_brojeva[a][2]), float(polje_brojeva[a][3]))
                Obj.Draw(self.C)

            elif polje_prvih_rijeci[a] == 'Circle':
                Obj = Kruznica(
                    polje_boja[a], polje_brojeva[a][0:2], polje_brojeva[a][2])
                Obj.Draw(self.C)

            elif polje_prvih_rijeci[a] == 'Ellipse':
                Obj = Elipsa(polje_boja[a], polje_brojeva[a][0:2],
                             polje_brojeva[a][2], polje_brojeva[a][3])
                Obj.Draw(self.C)

            else:
                pass

            a += 1

    def __init__(self, master=None):
        Frame.__init__(self, master)  # inicijaliziramo originalni Frame
        self.pack()
        self.CreateWidgets()  # Postavljamo naše kontrole


def StServer(C):
    if __name__ == '__main__':
            listensocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)  # Stvara socketobjekt
            port = 8000
            maxConnections = 10
            name = socket.gethostname()  # Naziv lokalnog računala
            listensocket.bind(('localhost', port))  # Može i '127.0.0.1'
            listensocket.listen(maxConnections)  # Pokreće server
            print("Startedserver at " + name + " on port " + str(port))
            C.create_text(550, 580, text="Started server at " + name + " on port " + str(port), fill="white", font=('Helvetica 15 bold'))
            C.pack()
            while True:
                (clientsocket, address) = listensocket.accept()
                print("New connectionmadefromaddress: ", address)
                clientsocket.sendall("Connection established.".encode())
                ClientConThr = thr.Thread(target=Srv_func, args=(clientsocket,C))
                ClientConThr.daemon = True
                ClientConThr.start()


def Srv_func(cs,C):  # Funkcija koja će se izvršavati za svakog klijenta u zasebnoj niti
    while True:
        # Dohvaća poslanu poruku i dekodira u string
        pass
        message = cs.recv(1024).decode()
        print(message)
        prva_rijec2 = message.split(' ')[0]
        druga_rijec2 = message.split(' ')[1]
        kordinate2 = message.split(' ')[2:]
        kordinate2[-1] = kordinate2[-1].strip()  # uklanja '/n'
        if kordinate2[-1] == '':
            kordinate2.pop()  # uklanja zadnji element linije ako je ''
            
        if prva_rijec2 == 'Polygon':
                Obj = Poligon(druga_rijec2, kordinate2[0:2], kordinate2[2:])
                Obj.Draw(C)

        elif prva_rijec2 == 'Line':
                Obj = Linija(druga_rijec2, kordinate2[0:2], kordinate2[2:])
                Obj.Draw(C)

        elif prva_rijec2 == 'Triangle':
                Obj = Trokut(druga_rijec2, kordinate2[0:2],kordinate2[2:4], kordinate2[4:])
                Obj.Draw(C)

        elif prva_rijec2 == 'Rectangle':

                Obj = Pravokutnik(druga_rijec2, kordinate2[0:2], float(kordinate2[2]), float(kordinate2[3]))
                Obj.Draw(C)

        elif prva_rijec2 == 'Circle':
                Obj = Kruznica(druga_rijec2, kordinate2[0:2], kordinate2[2])
                Obj.Draw(C)

        elif prva_rijec2 == 'Ellipse':
                Obj = Elipsa(druga_rijec2, kordinate2[0:2],kordinate2[2], kordinate2[3])
                Obj.Draw(C)

        else:
                pass    
        C.pack()




root = Tk()
app = Application(root)  # stvaramo našu klasu
app.mainloop()

