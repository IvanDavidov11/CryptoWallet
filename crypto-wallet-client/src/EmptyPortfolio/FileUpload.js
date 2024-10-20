import React, { useState, useEffect } from 'react'
import SpinnerLoader from '../SpinnerLoader';

const FileUpload = ({ onFileUpload, setUploadFormatFailed, setBadCoins, setIsLoading, isLoading }) => {
    const [file, setFile] = useState(null);
    const upload_ApiUrl = "https://localhost:7038/api/coins/upload";
    const uploadSafe_ApiUrl = "https://localhost:7038/api/coins/upload-safe";
    const [goodCoins, setGoodCoins] = useState([]);

    const handleFileChange = (event) => {
        setFile(event.target.files[0]);
    };

    const handleDragOver = (event) => {
        event.preventDefault();
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        if (file) {
            const formData = new FormData();
            formData.append('file', file);

            try {
                setIsLoading(true);
                const response = await fetch(upload_ApiUrl, {
                    method: 'POST',
                    body: formData,
                });

                if (!response.ok) throw new Error('File upload failed');

                if (response.status === 206) {
                    const result = await response.json();
                    setBadCoins(result.badCoins);
                    setGoodCoins(result.goodCoins);
                    setUploadFormatFailed(true);
                }
                else if (response.ok) {
                    const result = await response.text();
                    console.log(result);
                    onFileUpload();
                }

            } catch (error) {
                console.error('Error uploading file:', error);
            }
            finally {
                setIsLoading(false);
            }
        }
    };

    useEffect(() => {
        const sendGoodCoins = async () => {
            try {
                setIsLoading(true);
                const response = await fetch(uploadSafe_ApiUrl, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(goodCoins),
                });
    
                if (response.ok) {
                    const result = await response.text();
                    console.log(result);
                    onFileUpload();
                } else {
                    const error = await response.text();
                    console.error(error);
                }
            } catch (err) {
                console.error("An error occurred while sending good coins:", err);
            } finally {
                setIsLoading(false);
            }
        };
        if (goodCoins && goodCoins.length > 0) sendGoodCoins()
    }, [goodCoins]);

    const handleDrop = (event) => {
        event.preventDefault();
        const droppedFile = event.dataTransfer.files[0];
        if (droppedFile) {
            setFile(droppedFile);
        }
    };

    return (
        <>
            <form
                className="upload-box"
                onSubmit={handleSubmit}
                onDrop={handleDrop}
                onDragOver={handleDragOver}
            >
                {isLoading ? (
                    <SpinnerLoader />
                ) : (
                    <>
                        <label htmlFor="file-upload" className="drag-drop-text">
                            <strong>Drag & Drop</strong> <br /> or
                            <a href="#" onClick={() => document.getElementById('file-upload').click()}><strong> browse</strong></a> your file
                        </label>
                        <input
                            type="file"
                            id="file-upload"
                            accept=".txt, .csv" // Accepts text and CSV files
                            onChange={handleFileChange}
                            style={{ display: 'none' }}
                        />
                        <button
                            disabled={!file}
                            type='submit'
                            className="upload-button">
                            Upload
                        </button>
                    </>
                )}
            </form>

            <p className="file-types">
                Plain text (.txt, .csv) <br />
            </p>
        </>
    );
}

export default FileUpload
