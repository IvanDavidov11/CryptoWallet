import React from 'react'

const BadUploadHeader = () => {
    return (
        <>
            <h1>Some of the coins you uploaded are badly formatted or not found in Coin Lore database</h1>
            <p>Either re-upload the portfolio file, or just continue without the bad coins.</p>
        </>
    )
}

export default BadUploadHeader