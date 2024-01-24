import { useEffect, useState } from 'react';
import { useRouter } from 'next/router';
import { setFips } from 'crypto';

type Campo = {
    id: number;
    nombre: string;
};

type Libro = {
    id: number;
    titulo: string;
    descripcion: string;
    autorId: number;
    autor: Campo;
    editorialId: number;
    editorial: Campo;
    ciudad: string;
    codigoPostal: number;
    idPersona: number;
    free: number
};

type Persona = {
    id: number;
    username: string;
    ciudad: string;
    codigoPostal: number;
};

type Mensaje = {
    id: number;
    idChat: number;
    idPersona: number;
    persona?: Persona;
    texto: string;
};

type Chat = {
    id: number;
    idPersona1: number;
    idPersona2: number;
    persona1: Persona;
    persona2: Persona;
    libroId: number;
    libro: Libro;
    mensajes: Mensaje[];
};


const ChatPage = () => {
    const router = useRouter();
    const [id, setId] = useState<string>('');
    const [loading, setLoading] = useState<boolean>(true);
    const [chats, setChats] = useState<Chat[]>([]);
    const [mensajes, setMensajes] = useState<Mensaje[]>([]);
    const [cabecera, setCabecera] = useState<{ id: number; titulo: string; autor: string; username: string; ciudad: string; idChat:number }>();
    const [mensaje, setMensaje] = useState<string>('');
    const [index, setIndex] = useState<number>(0);
    const [loadingFree, setLoadingFree] = useState<boolean>(false);
    const [intervalo, setIntervalo] = useState<boolean>(false);

   
    useEffect(() => {
        // Cuando el componente se monta, obtenemos el id de la ruta actual y lo actualizamos en el estado
        setId(router.query.id?.toString() || "");


    }, [router.query.id]);
    useEffect(() => {
        if (id != "") {
            fetchTodo();
            
        }
    }, [id]); 
    useEffect(() => {
        if (cabecera) {
            if (cabecera.idChat)
                if (intervalo == false) {
                    console.log("iniciando intervalo")
                    setInterval(() => {
                        setIntervalo(true);
                        fetchMensajes();
                    }, 5000);
                }
                    
        }
    },[cabecera]);
    const fetchTodo = async () => {
        await fetchChatData();
        console.log("fetchTodo")
        
    }
    const fetchChatData = async () => {
        try {
            
            console.log(id)
            const url = `http://localhost:5105/persona/${id}`

            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            if (!response.ok) {
                console.error('Error fetching chat data:', response.status);
                return;
            }
            const data: Chat[] = await response.json();
            
            const chatsOrdenados = data.sort((a, b) => {
                const idMensajeA = Math.max(...a.mensajes.map(m => m.id));
                const idMensajeB = Math.max(...b.mensajes.map(m => m.id));

                return idMensajeB - idMensajeA;
            });
            console.log("chatsOrdenados")
            console.log(chatsOrdenados)

            setChats(chatsOrdenados)
            console.log("chats")
            console.log(chatsOrdenados);
            setMensajes(chatsOrdenados[index].mensajes);
            
            setCabecera({
                id: chatsOrdenados[index].libro.idPersona,
                idChat: chatsOrdenados[index].id,
                titulo: chatsOrdenados[index].libro.titulo,
                autor: chatsOrdenados[index].libro.autor.nombre,
                username: chatsOrdenados[index].persona2.username,
                ciudad: chatsOrdenados[index].persona2.ciudad,  
            });
            console.log(chatsOrdenados[index].libro.idPersona)
            setLoading(false);
            
        } catch (error) {
            console.error('Error fetching chat data:', error);
            console.log("error fetchchatdata")
        }
    };
    const handleNavigate = (path: string) => {
        router.push(`/${path}/${id}`);
    };
    const enviarMensaje = async () => {
        try { 
            console.log("chats2")
            console.log(chats);
            const response = await fetch(`http://localhost:5105/addMensaje?idChat=${cabecera?.idChat}&mensaje=${mensaje}&idUsuario=${id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            if (!response.ok) {
                console.error('Error enviando mensaje:', response.status);
                return;
            }
            await fetchMensajes();
        } catch (error) {
            console.error('Error enviando mensaje:', error);
        }
    };
    const fetchMensajes = async () => {
        console.log("fetchMensajes") 
        if(chats[index]){
            const response = await fetch(`http://localhost:5105/mensaje/${cabecera?.idChat}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            const data: Mensaje[] = await response.json();
            if (data.length != mensajes.length)
                setMensajes(data);
            console.log("dataset");
        }
    }
    const setFree = async (value: any) => {
        setLoadingFree(true);
        var num = 0;
        if (value)
            num = 1;
        const response = await fetch(`http://localhost:5101/libros`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: chats[index].libro.id,
                titulo: chats[index].libro.titulo,
                descripcion: chats[index].libro.descripcion,
                autorId: chats[index].libro.autorId,
                autor : chats[index].libro.autor,
                editorialId: chats[index].libro.editorialId,
                editorial: chats[index].libro.editorial,
                free: num,
                idPersona: chats[index].libro.idPersona,
            })
        });

        const response2 = await fetch(`http://localhost:5101/libros/${chats[index].libro.id}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        chats[index].libro = await response2.json();
        
        
        if (!response.ok) {
            console.error('Error enviando mensaje:', response.status);
            return;
        }
        setLoadingFree(false);
    }

    if (loading) {
        return <h1>Cargando...</h1>;
    }

    return (
        <>
            <div className="topBar">
                    <button className="botonTop1" onClick={() => handleNavigate('perfil')}>Perfil</button>
                    <button className="botonTop2" onClick={() => handleNavigate('main')}>Pagina principal</button>
            </div>
            <div className="chat-container">
                <div className="chat-list">
                    {chats.map((chat,index) => (
                        <div key={chat.id} className="chat-item" onClick={() => {
                            setIndex(index);
                            setMensajes(chat.mensajes);
                            setCabecera({
                                id: chat.libro.idPersona,
                                idChat: chat.id,
                                titulo: chat.libro.titulo,
                                autor: chat.libro.autor.nombre,
                                username: chat.persona2.username,
                                ciudad: chat.persona2.ciudad,
                            });
                            console.log(chats[index]);
                        }}>
                            <div className={`libroStatusIcon ${chats[index].libro.free === 1 ? 'libroStatusLibre' : 'libroStatusOcupado'}`} />
                            <div>
                                <p className="chatLibroTitleList">{chat.libro.titulo}</p>
                                <p className="chatP2UserList">{chat.persona2.username}</p>
                                <div className="divider"/>
                            </div>
                        </div>
                    ))}
                </div>
                <div className="chat-content">
                    <div className="chat-header">
                        <div className="cabeceraTitle">{cabecera?.titulo}</div>
                        <div className="cabeceraAutor">Autor: {cabecera?.autor}</div>
                        <div className="cabeceraPersona">Con: {cabecera?.username}</div>
                        <div className="cabeceraCiudad">Ciudad: {cabecera?.ciudad}</div>

                        {(cabecera && cabecera.id.toString() === id) && (
                            <div className="cabeceraCheck"> Libro libre:
                                {loadingFree ? <div>loading...</div>:
                                    <input className="cabeceraCheckInput"
                                    type="checkbox"
                                    name="El libro está libre"
                                    id="check"
                                    onChange={(e) => { setFree(e.target.checked) }}
                                    checked={chats[index].libro.free === 1}
                                />}
                                
                            </div>
                            
                        )}
                    </div>
                    <div className="chat-messages">
                        {mensajes.map((mensaje) => (
                            <div key={mensaje.id} className={`message ${mensaje.idPersona.toString() === id ? 'sent' : 'received'}`}>
                                <p>{mensaje.texto}</p>
                            </div>
                        ))}
                    </div>
                    <div className="chat-input">
                        <input type="text" placeholder="Escribe un mensaje" onChange={(e) => {
                            setMensaje(e.target.value)
                                
                        }} />
                        <button className="enviarMensaje" onClick={() => enviarMensaje()}>Enviar</button>
                    </div>
                </div>
            </div>
        </>
    );
};

export default ChatPage;