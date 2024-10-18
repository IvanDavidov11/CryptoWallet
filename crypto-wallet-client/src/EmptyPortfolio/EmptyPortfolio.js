import React from 'react'
import BadUpload from './BadUpload'
import BadUploadHeader from './BadUploadHeader'
import { useState } from 'react'
import FileUpload from './FileUpload'
import InitialUploadHeader from './InitialUploadHeader'

const EmptyPortfolio = ({ onFileUpload, setIsLoading, isLoading }) => {
  const [uploadFormatFailed, setUploadFormatFailed] = useState(false);
  const [goodCoins, setGoodCoins] = useState([]);
  const [badCoins, setBadCoins] = useState([]);

  return (
    <div>
      {uploadFormatFailed ? (
        <>
          <BadUploadHeader />
          <BadUpload
            goodCoins={goodCoins}
            setGoodCoins={setGoodCoins}
            badCoins={badCoins}
            setBadCoins={setBadCoins}
            onFileUpload={onFileUpload}
            setUploadFormatFailed={setUploadFormatFailed}
            setIsLoading={setIsLoading}
          />
          <FileUpload
            uploadCaption={"File isn't formatted correctly. Please re-upload."}
            onFileUpload={onFileUpload}
            setUploadFormatFailed={setUploadFormatFailed}
            setBadCoins={setBadCoins}
            setGoodCoins={setGoodCoins}
            setIsLoading={setIsLoading}
            isLoading={isLoading}
          />
        </>) : (
        <>
        <InitialUploadHeader />
          <FileUpload
            onFileUpload={onFileUpload}
            setUploadFormatFailed={setUploadFormatFailed}
            setBadCoins={setBadCoins}
            setGoodCoins={setGoodCoins}
            setIsLoading={setIsLoading}
            isLoading={isLoading}
          />
        </>)}
    </div>
  )
}

export default EmptyPortfolio