import socket

with socket.socket(socket.AF_INET,socket.SOCK_STREAM) as s:
	s.bind(("",50000))
	s.listen(5)
	while True:
		connection,addr=s.accept()
		with connection:
			print(addr)
			print(connection.recv(64))
