import { useEffect, useState } from 'react';

const BASE_API_URL = 'https://localhost:4004';

const useFetch = (url, method = 'GET', body = null) => {
    const [data, setData] = useState(null);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const init = async () => {
            try {
                if (body) {
                    const response = await fetch(BASE_API_URL + url, {
                        method: method.toUpperCase(),
                        mode: 'cors',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(body)
                    });
                    const json = await response.json();
                    setData(json);
                }
                else {
                    const response = await fetch(BASE_API_URL + url, {
                        method: method.toUpperCase()
                    });
                    const json = await response.json();
                    setData(json);
                }
                
            } catch (e) {
                setError(e);
            }
            finally {
                setLoading(false);
            }
        }

        init();
    }, [url]);
    return [ data, error, loading ];
};

export default useFetch;