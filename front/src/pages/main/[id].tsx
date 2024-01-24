import Head from 'next/head'
import Image from 'next/image'
import { Inter } from 'next/font/google'
import { useEffect, useState } from 'react'
import router, { useRouter } from 'next/router'
import { type } from 'os'

const inter = Inter({ subsets: ['latin'] })

export default function Main() {
    
    type campo = {
        id: number,
        nombre: string
    }
    type libro = {
        free: number
        id: number,
        titulo: string,
        descripcion: string
        autorId:number
        autor: campo
        editorialId:number
        editorial: campo
        ciudad: string
        codigoPostal: number
        idPersona : number
    }

    const [id, setId] = useState<string>('');
    const[porcentaje,setPorcentaje] = useState<number>(0)
    const [autores, setAutores] = useState<campo[]>([])
    const [editoriales, setEditoriales] = useState<campo[]>([])
    const [idAutor, setIdAutor] = useState<number>(0)
    const [idEditorial, setIdEditorial] = useState<number>(0)
    const [ciudad, setCiudad] = useState<string>("")
    const [codigoPostal, setCodigoPostal] = useState<number>(0)
    const [titulo, setTitulo] = useState<string>("")
    const [libros, setLibros] = useState<libro[]>()
    const [loading, setLoading] = useState<boolean>(true)
    const router = useRouter();

    useEffect(() => {
        // Cuando el componente se monta, obtenemos el id de la ruta actual y lo actualizamos en el estado
        setId(router.query.id?.toString() || "");
        fetchTodo()
        

        
    }, [router.query.id]);
    const handleNavigate = (path: string) => {
        router.push(`/${path}/${id}`);
    };
    
    const fetchTodo = async () => {

        await fetchCampos()
        setLoading(true);
        await fetchLibros()
        setLoading(false);
    }
    const fetchLibros = async () => {
        try {
            setLoading(true);
            const url = `http://localhost:5101/libros?autor=${idAutor}&editorial=${idEditorial}${ciudad!=""? "&ciudad="+ciudad:""}${codigoPostal!=0?"&codigoPostal="+codigoPostal:""}${titulo!=""?"&titulo="+titulo:""}`;
            console.log(url)
            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                throw new Error(`Error: ${response.status} - ${response.statusText}`);
            }

            const data:libro[] = await response.json();
            console.log("data")
            console.log(data)
            let index = 0;
            await Promise.all(data.map(async (libro) => {
                
                const respuestaPersona = await fetch(`http://localhost:5104/personas/${libro.idPersona}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                if (!respuestaPersona.ok) {
                    throw new Error(`Error: ${respuestaPersona.status} - ${respuestaPersona.statusText}`);
                }

                const persona = await respuestaPersona.json();
                libro.ciudad = persona.ciudad;
                libro.codigoPostal = persona.codigoPostal;
                setPorcentaje((index * 100) / data.length)
                index = index + 1;
            }));
            console.log("libros completos")
            console.log(data)
            await setLibros(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching libros:', error);
        }
    };
    const fetchCampos = async () => {
        try {
            var myHeaders = new Headers();
            myHeaders.append("Content-Type", "application/json");
            var requestOptions = {
                method: 'GET',
                headers: myHeaders
            };
            console.log("login4")
            const url = `http://localhost:5102/autores`;
            const response = await fetch(url, requestOptions)

            const url2 = `http://localhost:5103/editoriales`;
            const response2 = await fetch(url2, requestOptions)
            console.log(response)
            console.log(response2)
            console.log("json-izando")
            const data = await response.json();
            console.log("json-izando 2") 
            const data2 = await response2.json();
            console.log(data);
            console.log(data2);
            console.log("fetched")
            let listaAutores: campo[] = [{ id: 0, nombre: "autor" }]
            let listaEditoriales: campo[] = [{ id: 0, nombre: "editorial" }]
            listaAutores = [...listaAutores, ...data]
            listaEditoriales = [...listaEditoriales,...data2]
            setAutores(listaAutores)
            setEditoriales(listaEditoriales)

            
        } catch (e) {
        }
    }
    
    const chatButton=async (idLibro:number)=>{
        await fetch(`http://localhost:5105/chat?idUsuario=${id}&idLibro=${idLibro}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        handleNavigate('chats')
    }

    return (
        <div className="MainBody">
            <div className="topBar">
                <button className="botonTop1" onClick={() => handleNavigate('perfil')}>Perfil</button>
                <button className="botonTop2" onClick={() => handleNavigate('chats')}>Chats</button>
            </div>
            <div className="MainSearchSpace">
                <input className="MainSearchTitle" type="text" placeholder="Buscar..." onChange={(e) => { setTitulo(e.target.value) }} />
                <select className="MainSearchAutor"  onChange={(e) => setIdAutor(Number(e.target.value))}> 
                    {autores.map((filter, index) => (
                        <option key={index} value={filter.id}>
                            {filter.nombre}
                        </option>
                    ))}
                </select>
                <select className="MainSearchEditorial"  onChange={(e) => setIdEditorial(Number(e.target.value))}>
                    {editoriales.map((filter, index) => (
                        <option key={index} value={filter.id}>
                            {filter.nombre}
                        </option>
                    ))}
                </select>
                <input className="MainSearchCiudad" type="text" placeholder="ciudad" onChange={(e) => setCiudad(e.target.value)} />
                <input className="MainSearchCodigoPostal" type="number" placeholder="C.P" onChange={(e) => setCodigoPostal(Number(e.target.value))} />
                <button className="botonSearch" onClick={()=>fetchLibros()} >Buscar</button>
            </div>
            <div className="listOrLoading">
                {loading ? <div className="loading" >Cargando {" "+Math.trunc(porcentaje)+"%"}</div> :
                    <div className="bookListMain">
                    <h2 className="bookListTitleMain">Lista de Libros</h2>
                    <div className="libro">
                            {libros?.map((libro) => {
                                if (libro.idPersona.toString()!=id&&libro.free==1)
                                    return (
                                        <div className="LibroYBoton">
                                            <div className="libroIn" key={libro.id}>
                                                <div className="MainBookTit">{libro.titulo}</div>
                                                <div className="DataYCiudad">
                                                    <div className="LibroData">
                                                        <div className="MainBookDesc">{libro.descripcion}</div>
                                                        <div className="MainBookAuth">Autor: {libro.autor.nombre}</div>
                                                        <div className="MainBookEd">Editorial: {libro.editorial.nombre}</div>
                                                    </div>
                                                    <div className="DatosCiudad">
                                                        <div className="MainBookCiu">Ciudad: {libro.ciudad}</div>
                                                        <div className="MainBookCP">{libro.codigoPostal ? "Codigo Postal: " + libro.codigoPostal : ""}</div>
                                                    </div>
                                                </div>
                                            </div>
                                            <button className="MainChatButton" onClick={() => chatButton(libro.id)}>Iniciar Chat</button>
                                        </div>
                                    )
                            })}
                    </div>
                </div> }
            </div>
            
        </div>

    )
}