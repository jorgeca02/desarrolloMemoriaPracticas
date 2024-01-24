import Head from 'next/head'
import Image from 'next/image'
import { Inter } from 'next/font/google'
import { useEffect, useState } from 'react'
import router, { useRouter } from 'next/router'
import { type } from 'os'

const inter = Inter({ subsets: ['latin'] })

export default function Perfil() {
    
    type campo = {
        id: number,
        nombre: string
    }
    type libro = {
        free: number
        id: number,
        titulo: string,
        descripcion: string
        autorId: number
        autor: campo
        editorialId: number
        editorial: campo
        idPersona : number
    }
    type persona = {
        id: number,
        username: string,
        password: string,
        codigoPostal: number,
        ciudad: string,
        libros: libro[]
    }

    const [id, setId] = useState<string>('');
const [PwMsg, setPwMsg] = useState<string>('');
    const [persona, setPersona] = useState<persona>();
    const [loading, setLoading] = useState<boolean>(true)
    const [loadingFree, setLoadingFree] = useState<boolean>(false);
    const router = useRouter();
    const [formData, setFormData] = useState({
        username: '',
        password: '',
        password2: '',
        codigoPostal: 0,
        ciudad: '',
    });

    useEffect(() => {
        if (persona) {
            setFormData({
                username: persona.username,
                password: persona.password,
                password2: "",
                codigoPostal: persona.codigoPostal,
                ciudad: persona.ciudad,
            });
        }
    }, [persona,loading,id]);
    useEffect(() => {
        // Cuando el componente se monta, obtenemos el id de la ruta actual y lo actualizamos en el estado
        console.log(router.query.id?.toString() || "no hay id")
        setId(router.query.id?.toString() || "");
        if(id != ""&& !persona)
            fetchPersona()
    }, [router.query.id]);
    useEffect(() => {
        if (id != "" && !persona)
            fetchPersona()
    }, [id]);
    const handleNavigate = (path: string) => {
        router.push(`/${path}/${id}`);
    };
    const setFree = async (value:any,libro:libro) => {
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
                id: libro.id,
                titulo: libro.titulo,
                descripcion: libro.descripcion,
                autorId: libro.autorId,
                autor : libro.autor,
                editorialId: libro.editorialId,
                editorial: libro.editorial,
                free: num,
                idPersona: libro.idPersona,
            })
        });

        const response2 = await fetch(`http://localhost:5101/libros/${libro.id}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        libro = await response2.json();
        
        
        if (!response.ok) {
            console.error('Error enviando mensaje:', response.status);
            return;
        }
        setLoadingFree(false);
        fetchPersona();
    }
    const fetchPersona = async () => {
        try {
            setLoading(true);
            console.log(`http://localhost:5104/personas/${id}`)
            if (id) {
                const response = await fetch(`http://localhost:5104/personas/${id}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error(`Error: ${response.status} - ${response.statusText}`);
                }

                const data = await response.json();
                console.log(data)
                setPersona(data);
            }
            setLoading(false);
        } catch (error) {
            console.error('Error fetching libros:', error);
        }
    };
    const handleFormChange = (e: { target: { name: any; value: any } }) => {
        // Actualizar el estado del formulario al cambiar los datos
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };
    
    const handleUpdatePersona = async () => {
        // Lógica para enviar la actualización de la persona al servidor
        try {
            
            console.log("fetching")
            const response = await fetch(`http://localhost:5104/personas`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(
                    {
                        id: id
                        , username: formData.username
                        , password: persona?.password  
                        , codigoPostal: formData.codigoPostal
                        , ciudad: formData.ciudad
                    }),
            });

            if (!response.ok) {
                throw new Error(`Error: ${response.status} - ${response.statusText}`);
            }

            // Actualizar la información de la persona después de la actualización
            fetchPersona();
        } catch (error) {
            console.error('Error updating persona:', error);
        }
    };
    const deleteRel = async (id: number) => {
        setLoading(true);
        await fetch(`http://localhost:5101/libros/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
        });
        fetchPersona();
    }
    const handleUpdatePassword = async (e: any) => {
        e.preventDefault()
        console.log(PwMsg)
// Lógica para enviar la actualización de la persona al servidor
        try {
            console.log("handleupdatepassword")
            console.log(formData.password == formData.password2)
            if (formData.password == formData.password2) {
                console.log("fetching")
                const response = await fetch(`http://localhost:5104/personas`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(
                        {
                            id: id
                            , username: persona?.username
                            , password: formData.password
                            , codigoPostal: formData.codigoPostal
                            , ciudad: formData.ciudad
                        }),
                });

                if (!response.ok) {
                    throw new Error(`Error: ${response.status} - ${response.statusText}`);
                }

                // Actualizar la información de la persona después de la actualización
                setPwMsg("")
                fetchPersona();
            }
            else {
                setPwMsg("Las contraseñas no coinciden")
                console.log(PwMsg)
                return 
            }
        } catch (error) {
            console.error('Error updating persona:', error);
        }
        console.log(PwMsg)
    }
    return (
        <>
            <div className="topBar">
                <button className="botonTop1" onClick={() => handleNavigate('main')}>Pagina principal</button>
                <button className="botonTop2" onClick={() => handleNavigate('chats')}>Chats</button>
            </div>
            
            <div >
                {loading ? <div className="loadingProfile" >Cargando...</div> :
                    <div className="ProfileContainer">
                        <div className="ProfileForm">
                            <form>
                                <label>Username:</label>
                                <input className="inputProfile"
                                    type="text"
                                    name="username"
                                    value={formData.username}
                                    onChange={handleFormChange}
                                />
                                <label>Contraseña:</label>
                                    <input className="inputProfile"
                                    type="password"
                                    name="password"
                                    value={formData.password}
                                    onChange={handleFormChange}
                                />
                                <label>Repetir Contraseña:</label>
                                <input className="inputProfile"
                                    type="password"
                                    name="password2"
                                    value={formData.password2}
                                    onChange={handleFormChange}
                                />
                                <button onClick={handleUpdatePassword}>Actualizar Contraseña</button>
                                <div>{PwMsg}</div>
                                <label>Código Postal:</label>
                                    <input className="inputProfile"
                                    type="number"
                                    name="codigoPostal"
                                    value={formData.codigoPostal}
                                    onChange={handleFormChange}
                                />
                                <label>Ciudad:</label>
                                    <input className="inputProfile" 
                                    type="text"
                                    name="ciudad"
                                    value={formData.ciudad}
                                    onChange={handleFormChange}
                                />
                                    <button type="button" onClick={handleUpdatePersona} className="inputProfile">
                                    Actualizar Perfil
                                </button>
                            </form>
                        </div>
                        <div className="ProfileBookList">
                            <div className="bookList" >
                                <h2 className="bookListTitle">Lista de Libros</h2>
                                <ul>
                                    {persona?.libros?.map((libro) =>
                                    {
                                    

                                        return (    
                                            <div className="LibroPers">
                                                
                                                <div key={libro.id} className="LibroPersContent">
                                                    <div className="LibroPersTitle">{libro.titulo}</div>
                                                    <p className="LibroPersDescription">{libro.descripcion}</p>
                                                    <p className="LibroPersAuthor">Autor: {libro.autor.nombre}</p>
                                                    <p className="LibroPersEditorial">Editorial: {libro.editorial.nombre}</p>
                                                    {loadingFree ? <div>loading...</div>:
                                                        <>
                                                            <div>libre:</div>
                                                            <input className="CheckInput"
                                                            type="checkbox"
                                                            name="El libro está libre"
                                                            id="check"
                                                            onChange={(e) => { setFree(e.target.checked,libro) }}
                                                            checked={libro.free === 1}
                                                            />
                                                        </>}
                                                </div>
                                                <button className="deleteButton" onClick={() => deleteRel(libro.id)}>Eliminar</button>
                                            </div>
                                        )
                                    })}
                                </ul>
                            </div>
                            <button type="button" className="addLibroButton" onClick={() => handleNavigate('addLibro')}>Añadir Libro</button>
                        </div>
                    </div>}
            </div>
            
        </>
    );
}
