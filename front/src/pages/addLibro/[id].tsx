import Head from 'next/head';
import { cache, useEffect, useState } from 'react';
import { useRouter } from 'next/router';

export default function AgregarLibro() {
    type campo = {
        id: number
        nombre: string
    }
    const { id } = useRouter().query;

    const [titulos, setTitulos] = useState<campo[]>([]);
    const [autores, setAutores] = useState<campo[]>([]);
    const [editoriales, setEditoriales] = useState<campo[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    const [mensaje, setMensaje] = useState<string>('');

    const [titulo, setTitulo] = useState<string>();
    const [descripcion, setDescripcion] = useState<string>();
    const [autor, setAutor] = useState<number>();
    const [editorial, setEditorial] = useState<number>();

    const [autorNuevo, setAutorNuevo] = useState<string>();
    const [editorialNueva, setEditorialNueva] = useState<string>();

    const [showNuevoAutor, setShowNuevoAutor] = useState(false);
    const [showNuevaEditorial, setShowNuevaEditorial] = useState(false);

    const router = useRouter();

    useEffect(() => {
        fetchAutores();
        fetchEditoriales();
    }, []);



    const fetchEditoriales = async () => {
        try {
            const response = await fetch(`http://localhost:5103/editoriales`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            if (response.ok) {
                setEditoriales(await response.json());
            }
            else {
                setMensaje('Error al cargar editoriales');
            }
        } catch (e) {
            setMensaje('Error al cargar editoriales');
        }
    };

    const fetchAutores = async () => {
        try {
            const response = await fetch(`http://localhost:5102/autores`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            if (response.ok) {
                setAutores(await response.json());
            }
            else {
                setMensaje('Error al cargar autores');
            }
        } catch (e) {
            setMensaje('Error al cargar autores');
        }
    }
    const createEditorial = async () => {
        try {
            if (!editorialNueva) {
                setMensaje('Faltan datos');
                return;
            }

            var myHeaders = new Headers();
            myHeaders.append("Content-Type", "application/json");
            var raw = JSON.stringify({
                "nombre": editorialNueva,
            });
            console.log(raw);
            var requestOptions = {
                method: 'POST',
                headers: myHeaders,
                body: raw,
            };

            const response = await fetch("http://localhost:5103/editoriales", requestOptions)
            console.log("fetch hecho")
            const data = await response.json();
            console.log(data);
            if (response.ok) {
                fetchEditoriales();
            }
        } catch (e) {
            fetchEditoriales();
        }
    }
    const createAutor = async () => {
        try {
            if (!autorNuevo) {
                setMensaje('Faltan datos');
                return;
            }

            var myHeaders = new Headers();
            myHeaders.append("Content-Type", "application/json");
            var raw = JSON.stringify({
                "nombre": autorNuevo,
            });
            console.log(raw);
            var requestOptions = {
                method: 'POST',
                headers: myHeaders,
                body: raw,
            };

            const response = await fetch("http://localhost:5102/autores", requestOptions)
            console.log("fetch hecho")
            console.log(response)
            const data = await response.json();
            console.log(data);
            if (data) {
                fetchAutores();
            }
        } catch (e) {
            fetchAutores();
        }
    }
    const addLibro = async (titulo: string | undefined, descripcion: string | undefined, autor: number | undefined, editorial: number | undefined) => {
        console.log({ titulo, descripcion, autor, editorial })
        try {
            if (!titulo || !autor || !editorial || !descripcion) {
                setMensaje('Faltan datos');
                return;
            }

            var myHeaders = new Headers();
            myHeaders.append("Content-Type", "application/json");
            var raw = JSON.stringify({
                "titulo": titulo,
                "autorId": autor,
                "descripcion": descripcion,
                "editorialId": editorial,
                "idPersona": id,
                "free": 1,
            });
            console.log(raw);
            var requestOptions = {
                method: 'POST',
                headers: myHeaders,
                body: raw,
            };

            const response = await fetch("http://localhost:5101/libros", requestOptions)
            console.log("fetch hecho")
            const data = await response.json();

            console.log(data);
            if (data) {
                router.push(`/perfil/${id}`);
            } else {
                setMensaje(data.mensaje);
            }
        } catch (e) {
            console.log(e);
            setMensaje("Error al crear libro");
        }
    }

    return (
        <>
            <Head>
                <title>Agregar Libro</title>
            </Head>

            <div className="topBar">
                <button className="botonTop1" onClick={() => router.push(`/perfil/${id}`)}>
                    Perfil
                </button>
                <button className="botonTop2" onClick={() => router.push(`/main${id}`)}>
                    Pagina Principal
                </button>
            </div>

            <div className="formContainer">
                <div className="inputGroup">
                    <label className="inputLabel">TÃ­tulo:</label>
                    <input
                        type="text"
                        className="selectDropdown"
                        placeholder="Tí­tulo"
                        onChange={(e) => { setTitulo(e.target.value) }}
                    />
                </div>

                <div className="inputGroup">
                    <label className="inputLabel">DescripciÃ³n:</label>
                    <input
                        type="text"
                        className="selectDropdown"
                        placeholder="Descripción"
                        onChange={(e) => { setDescripcion(e.target.value) }}
                    />
                </div>

                <div className="inputGroup">
                    <label className="inputLabel">Autor:</label>
                    <select
                        className="selectDropdown"
                        onChange={(e) => { setAutor(Number(e.target.value)) }}
                    >
                        {autores.map((autor, index) => (
                            <option key={index} value={autor.id}>{autor.nombre}</option>
                        ))}
                    </select>
                    <button
                        type="button"
                        onClick={() => { setShowNuevoAutor(!showNuevoAutor) }}
                        className="addButton"
                    >
                        AÃ±adir Nuevo Autor
                    </button>
                </div>

                <div className="inputGroup">
                    <label className="inputLabel">Editorial:</label>
                    <select
                        className="selectDropdown"
                        onChange={(e) => { setEditorial(Number(e.target.value)) }}
                    >
                        {editoriales.map((editorial, index) => (
                            <option key={index} value={editorial.id}>{editorial.nombre}</option>
                        ))}
                    </select>
                    <button
                        type="button"
                        onClick={() => { setShowNuevaEditorial(!showNuevaEditorial) }}
                        className="addButton"
                    >
                        Añadir Nueva Editorial
                    </button>
                </div>

                <button
                    type="button"
                    onClick={() => { addLibro(titulo, descripcion, autor, editorial) }}
                    className="submitButton"
                >
                    Añadir Libro
                </button>
            </div>

            {showNuevoAutor && (
                <div className="modal">
                    <div className="modalContent">
                        <label className="inputLabel">Nuevo Autor:</label>
                        <input
                            type="text"
                            name="nuevoAutor"
                            onChange={(e) => { setAutorNuevo(e.target.value) }}
                        />
                        <button
                            onClick={() => { setShowNuevoAutor(!showNuevoAutor) }}
                            className="closeButton"
                        >
                            Cerrar
                        </button>
                        <button
                            onClick={() => { createAutor(); setShowNuevoAutor(!showNuevoAutor); }}
                            className="closeButton"
                        >
                            Crear
                        </button>
                    </div>
                </div>
            )}

            {showNuevaEditorial && (
                <div className="modal">
                    <div className="modalContent">
                        <label className="inputLabel">Nueva Editorial:</label>
                        <input
                            type="text"
                            name="nuevaEditorial"
                            onChange={(e) => { setEditorialNueva(e.target.value) }}
                        />
                        <button
                            onClick={() => { setShowNuevaEditorial(!showNuevaEditorial) }}
                            className="closeButton"
                        >
                            Cerrar
                        </button>
                        <button
                            onClick={() => { createEditorial(); setShowNuevaEditorial(!showNuevaEditorial); }}
                            className="closeButton"
                        >
                            Crear
                        </button>
                    </div>
                </div>
            )}

            <div>{mensaje}</div>
        </>
    );
}

