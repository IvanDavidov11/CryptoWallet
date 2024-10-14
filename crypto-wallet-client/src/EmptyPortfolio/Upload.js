import React from 'react'
import { useState } from 'react';

const Upload = ({ uploadCaption, onFileUpload, setUploadFormatFailed, setBadCoins, setGoodCoins }) => {
    const [file, setFile] = useState(null);
    const upload_ApiUrl = "https://localhost:7038/api/coins/upload";

    const handleFileChange = (event) => {
        setFile(event.target.files[0]);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        if (file) {
            const formData = new FormData();
            formData.append('file', file);

            try {
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
                    console.log(`${response.status} - ${result.badCoins}`);
                }
                else if (response.ok) {
                    const result = await response.text();
                    console.log(result);
                    onFileUpload();
                }

            } catch (error) {
                console.error('Error uploading file:', error);
            }
        }
    };

    return (
        <form className='upload-form' onSubmit={handleSubmit}>
            <label htmlFor="file-upload">{uploadCaption}</label>
            <input
                type="file"
                id="file-upload"
                accept=".txt, .csv" // Accepts text and CSV files
                onChange={handleFileChange}
            />
            <button type="submit">Upload</button>
        </form>
    );
};

export default Upload